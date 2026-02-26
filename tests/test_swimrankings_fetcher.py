import unittest

from tools.swimrankings_fetcher import parse_athlete_page, parse_date, parse_swim_time


SAMPLE_HTML = '''
<div id="name">DOE, JOHN<br>(1986)&nbsp;</div>
<div id="nationclub"><br>NED<br>ENC Arnhem</div>
<img src="images/gender1.png">
<table class="athleteBest">
<tr class="athleteBest0">
  <td class="event"><a href="#">50 m Freestyle</a></td>
  <td class="course">50m</td>
  <td><a  class="time" href="#">28.39</a></td>
  <td class="name"><a title="Open Nederlandse Masters Kampioenschappen 2018 lb">ONMK</a></td>
  <td class="date">04.05.2018</td>
  <td class="city"><a title="Den Haag">Den Haag</a></td>
</tr>
<tr class="athleteBest1">
  <td class="event"><a href="#">200 m Butterfly</a></td>
  <td class="course">25m</td>
  <td><a  class="time" href="#">2:15.15</a></td>
  <td class="name"><a title="Winter Meet">Winter Meet</a></td>
  <td class="date">12.01.2019</td>
  <td class="city"><a title="Arnhem">Arnhem</a></td>
</tr>
</table>
'''


class SwimrankingsFetcherTests(unittest.TestCase):
    def test_parses_swimmer_and_pbs(self):
        data = parse_athlete_page(SAMPLE_HTML, "4046710")

        self.assertEqual(data.first_name, "John")
        self.assertEqual(data.last_name, "Doe")
        self.assertEqual(data.year_of_birth, 1986)
        self.assertEqual(data.club, "ENC Arnhem")
        self.assertEqual(data.gender, "Male")
        self.assertEqual(len(data.pbs), 2)
        self.assertEqual(data.pbs[0].swim_time["time_in_ms"], 28390)
        self.assertEqual(data.pbs[1].swim_time["time_in_ms"], 135150)

    def test_parse_swim_time(self):
        self.assertEqual(parse_swim_time("58.95")["time_in_ms"], 58950)
        self.assertEqual(parse_swim_time("1:06.31")["time_in_ms"], 66310)

    def test_parse_date(self):
        self.assertEqual(parse_date("04.05.2018"), {"day": 4, "month": 5, "year": 2018})


if __name__ == "__main__":
    unittest.main()
