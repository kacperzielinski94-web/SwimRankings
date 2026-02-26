#!/usr/bin/env python3
"""Fetch and parse swimmer data from swimrankings.net without external dependencies."""

from __future__ import annotations

import argparse
import json
import re
from dataclasses import asdict, dataclass, field
from datetime import datetime
from html import unescape
from typing import Any
from urllib.error import HTTPError, URLError
from urllib.request import Request, urlopen

BASE_URL = "https://www.swimrankings.net/index.php?page=athleteDetail&athleteId={athlete_id}&language={language}"


@dataclass
class Meet:
    name: str = ""
    date: dict[str, int] = field(default_factory=lambda: {"day": 0, "month": 0, "year": 0})
    city: str = ""


@dataclass
class Pb:
    stroke: str = "Unknown"
    distance_in_meters: int = 0
    pool_length: int = 0
    swim_time: dict[str, Any] = field(default_factory=lambda: {"time_in_ms": 0, "display_value": ""})
    meet: Meet = field(default_factory=Meet)


@dataclass
class SwimmerData:
    swimrankings_id: str
    first_name: str = "Unknown"
    last_name: str = "Unknown"
    year_of_birth: int = 0
    gender: str = "Unknown"
    club: str = ""
    pbs: list[Pb] = field(default_factory=list)


def _strip_tags(value: str) -> str:
    return unescape(re.sub(r"<[^>]+>", "", value or "")).strip()


def _match(text: str, pattern: str, default: str = "") -> str:
    m = re.search(pattern, text, re.S | re.I)
    return m.group(1).strip() if m else default


def _title_case_name(name: str) -> str:
    return " ".join(part.capitalize() for part in name.split()) if name else ""


def parse_swim_time(value: str) -> dict[str, Any]:
    value = value.strip()
    if not value:
        return {"time_in_ms": 0, "display_value": ""}

    if ":" in value:
        minutes_str, sec_part = value.split(":", 1)
        sec, _, hundredths = sec_part.partition(".")
        ms = (int(minutes_str) * 60 + int(sec)) * 1000 + int((hundredths or "0").ljust(2, "0")[:2]) * 10
    else:
        sec, _, hundredths = value.partition(".")
        ms = int(sec) * 1000 + int((hundredths or "0").ljust(2, "0")[:2]) * 10

    return {"time_in_ms": ms, "display_value": value}


def parse_date(value: str) -> dict[str, int]:
    value = value.strip()
    for fmt in ("%d.%m.%Y", "%d/%m/%Y", "%Y-%m-%d"):
        try:
            dt = datetime.strptime(value, fmt)
            return {"day": dt.day, "month": dt.month, "year": dt.year}
        except ValueError:
            continue
    return {"day": 0, "month": 0, "year": 0}


def _extract_td(row_html: str, class_name: str) -> str:
    patterns = [
        rf'<td[^>]*class="[^"]*{class_name}[^"]*"[^>]*>(.*?)</td>',
        rf"<td[^>]*class='[^']*{class_name}[^']*'[^>]*>(.*?)</td>",
    ]
    for pattern in patterns:
        found = _match(row_html, pattern)
        if found:
            return found
    return ""


def parse_athlete_page(page_contents: str, athlete_id: str) -> SwimmerData:
    swimmer = SwimmerData(swimrankings_id=athlete_id)

    athlete_block = _match(page_contents, r'<div[^>]*id="name"[^>]*>(.*?)</div>')
    if athlete_block:
        year_of_birth = _match(athlete_block, r"\((\d{4})\)", "0")
        name_part = _strip_tags(athlete_block.replace(f"({year_of_birth})", ""))
        if "," in name_part:
            last_name, first_name = [x.strip() for x in name_part.split(",", 1)]
            swimmer.first_name = _title_case_name(first_name)
            swimmer.last_name = _title_case_name(last_name)
        swimmer.year_of_birth = int(year_of_birth)

    nationclub = _match(page_contents, r'<div[^>]*id="nationclub"[^>]*>(.*?)</div>')
    if nationclub:
        lines = [_strip_tags(line) for line in re.split(r"<br\s*/?>", nationclub) if _strip_tags(line)]
        if lines:
            swimmer.club = lines[-1]

    if "images/gender1.png" in page_contents:
        swimmer.gender = "Male"
    elif "images/gender2.png" in page_contents:
        swimmer.gender = "Female"

    pb_table = _match(page_contents, r'<table[^>]*class="[^"]*athleteBest[^"]*"[^>]*>(.*?)</table>')
    if pb_table:
        rows = re.findall(r"<tr[^>]*class=\"[^\"]*athleteBest[^\"]*\"[^>]*>(.*?)</tr>", pb_table, re.S | re.I)
        for row in rows:
            event_cell = _extract_td(row, "event")
            stroke_and_distance = _strip_tags(event_cell)
            distance = _match(stroke_and_distance, r"(\d+)\s*m", "0")
            stroke = _match(stroke_and_distance, r"\d+\s*m\s*(.*)$", "Unknown")

            pool_length = _strip_tags(_extract_td(row, "course")).replace("m", "").strip()
            time_string = _strip_tags(_extract_td(row, "time"))
            if not time_string:
                time_string = _strip_tags(_match(row, r'<a[^>]*class="[^"]*time[^"]*"[^>]*>(.*?)</a>'))
            name_cell = _extract_td(row, "name")
            date_cell = _strip_tags(_extract_td(row, "date"))
            city_cell = _extract_td(row, "city")

            meet_title = _match(name_cell, r'title="(.*?)"') or _strip_tags(name_cell)
            city_title = _match(city_cell, r'title="(.*?)"') or _strip_tags(city_cell)

            swimmer.pbs.append(
                Pb(
                    stroke=stroke,
                    distance_in_meters=int(distance) if distance.isdigit() else 0,
                    pool_length=int(pool_length) if pool_length.isdigit() else 0,
                    swim_time=parse_swim_time(time_string),
                    meet=Meet(name=unescape(meet_title), date=parse_date(unescape(date_cell)), city=unescape(city_title)),
                )
            )

    return swimmer


def fetch_page(athlete_id: str, language: str = "us", timeout: int = 25) -> str:
    url = BASE_URL.format(athlete_id=athlete_id, language=language)
    request = Request(url, headers={"User-Agent": "Mozilla/5.0 (compatible; SwimRankingsFetcher/1.0)"})
    with urlopen(request, timeout=timeout) as response:
        return response.read().decode("utf-8", errors="replace")


def main() -> int:
    parser = argparse.ArgumentParser(description="Fetch swimmer profile from swimrankings.net")
    parser.add_argument("athlete_id", help="Athlete ID used by swimrankings.net")
    parser.add_argument("--language", default="us", help="Language code (default: us)")
    parser.add_argument("--timeout", type=int, default=25, help="HTTP timeout in seconds")
    parser.add_argument("--pretty", action="store_true", help="Pretty-print JSON")
    args = parser.parse_args()

    try:
        page_contents = fetch_page(args.athlete_id, args.language, timeout=args.timeout)
        swimmer = parse_athlete_page(page_contents, args.athlete_id)
        payload = asdict(swimmer)
        print(json.dumps(payload, ensure_ascii=False, indent=2 if args.pretty else None))
        return 0
    except HTTPError as exc:
        print(json.dumps({"error": f"HTTP {exc.code}", "details": str(exc)}))
        return 2
    except URLError as exc:
        print(json.dumps({"error": "NetworkError", "details": str(exc)}))
        return 2


if __name__ == "__main__":
    raise SystemExit(main())
