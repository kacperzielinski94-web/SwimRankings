# SwimRankings
Download swimmer data directly from SwimRankings.com

## Features

- Fetch individual swimmer data with personal bests
- Retrieve all swimmers from a specific club
- Filter swimmers by performance criteria (minimum times)
- Optimize relay team selection (freestyle and medley relays)
- Export data in JSON format for n8n workflows and automation
- Manage and select swimmers for competitions

## Quick Start

### Fetch Individual Swimmer Data

Example code:
```csharp
var swimrankingsId = "4046710";

var httpClient = new HttpClient();
var api = new SwimmerApi(httpClient);

var swimmer = await api.GetAsync(swimrankingsId);

Console.WriteLine(JsonSerializer.Serialize(swimmer, new JsonSerializerOptions { WriteIndented = true }));
```

This will write the following to the console:
```json
{
  "Gender": "Male",
  "Club": "ENC Arnhem",
  "YearOfBirth": 1986,
  "Pbs": [
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 28390,
        "DisplayValue": "28.39"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "Open Nederlandse Masters Kampioenschappen 2018 lb",
        "Date": {
          "Day": 4,
          "Month": 5,
          "Year": 2018
        },
        "City": "Den\u00A0Haag"
      }
    },
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 25790,
        "DisplayValue": "25.79"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 9,
          "Month": 3,
          "Year": 2008
        },
        "City": "Arnhem"
      }
    },
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 58950,
        "DisplayValue": "58.95"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 29,
          "Month": 5,
          "Year": 2005
        },
        "City": "Nijmegen"
      }
    },
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 56500,
        "DisplayValue": "56.50"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 15,
          "Month": 9,
          "Year": 2007
        },
        "City": "Veenendaal"
      }
    },
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 200,
      "SwimTime": {
        "TimeInMs": 135150,
        "DisplayValue": "2:15.15"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 12,
          "Month": 1,
          "Year": 2019
        },
        "City": "Wageningen"
      }
    },
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 400,
      "SwimTime": {
        "TimeInMs": 293040,
        "DisplayValue": "4:53.04"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "Deel 1 KNZB zwemcompetitie landelijk B/C",
        "Date": {
          "Day": 29,
          "Month": 9,
          "Year": 2007
        },
        "City": "Doetinchem"
      }
    },
    {
      "Stroke": "Freestyle",
      "DistanceInMeters": 800,
      "SwimTime": {
        "TimeInMs": 673460,
        "DisplayValue": "11:13.46"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "A. Jansen Lange Afstand Circuit  periode 1",
        "Date": {
          "Day": 8,
          "Month": 12,
          "Year": 2024
        },
        "City": "Doetinchem"
      }
    },
    {
      "Stroke": "Backstroke",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 31910,
        "DisplayValue": "31.91"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "Open Nederlandse Masters Kampioenschappen 2018 lb",
        "Date": {
          "Day": 4,
          "Month": 5,
          "Year": 2018
        },
        "City": "Den\u00A0Haag"
      }
    },
    {
      "Stroke": "Backstroke",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 31050,
        "DisplayValue": "31.05"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 13,
          "Month": 11,
          "Year": 2022
        },
        "City": "Arnhem"
      }
    },
    {
      "Stroke": "Backstroke",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 66310,
        "DisplayValue": "1:06.31"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 21,
          "Month": 5,
          "Year": 2005
        },
        "City": "Nijmegen"
      }
    },
    {
      "Stroke": "Backstroke",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 67410,
        "DisplayValue": "1:07.41"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 9,
          "Month": 3,
          "Year": 2008
        },
        "City": "Arnhem"
      }
    },
    {
      "Stroke": "Backstroke",
      "DistanceInMeters": 200,
      "SwimTime": {
        "TimeInMs": 154460,
        "DisplayValue": "2:34.46"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 29,
          "Month": 5,
          "Year": 2005
        },
        "City": "Nijmegen"
      }
    },
    {
      "Stroke": "Backstroke",
      "DistanceInMeters": 200,
      "SwimTime": {
        "TimeInMs": 149360,
        "DisplayValue": "2:29.36"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 9,
          "Month": 2,
          "Year": 2008
        },
        "City": "Wapenveld"
      }
    },
    {
      "Stroke": "Breaststroke",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 34110,
        "DisplayValue": "34.11"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 23,
          "Month": 6,
          "Year": 2018
        },
        "City": "Westervooort"
      }
    },
    {
      "Stroke": "Breaststroke",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 81420,
        "DisplayValue": "1:21.42"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "Deel 1 KNZB zwemcompetitie landelijk B/C",
        "Date": {
          "Day": 29,
          "Month": 9,
          "Year": 2007
        },
        "City": "Doetinchem"
      }
    },
    {
      "Stroke": "Breaststroke",
      "DistanceInMeters": 200,
      "SwimTime": {
        "TimeInMs": 183010,
        "DisplayValue": "3:03.01"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 23,
          "Month": 2,
          "Year": 2019
        },
        "City": "Westervoort"
      }
    },
    {
      "Stroke": "Butterfly",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 29450,
        "DisplayValue": "29.45"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "Open Nederlandse Masters Kampioenschappen 2018 lb",
        "Date": {
          "Day": 6,
          "Month": 5,
          "Year": 2018
        },
        "City": "Den\u00A0Haag"
      }
    },
    {
      "Stroke": "Butterfly",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 27950,
        "DisplayValue": "27.95"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 9,
          "Month": 2,
          "Year": 2008
        },
        "City": "Wapenveld"
      }
    },
    {
      "Stroke": "Butterfly",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 71850,
        "DisplayValue": "1:11.85"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 17,
          "Month": 3,
          "Year": 2018
        },
        "City": "Wageningen"
      }
    },
    {
      "Stroke": "Medley",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 65480,
        "DisplayValue": "1:05.48"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "Deel 1 KNZB zwemcompetitie landelijk B/C",
        "Date": {
          "Day": 29,
          "Month": 9,
          "Year": 2007
        },
        "City": "Doetinchem"
      }
    },
    {
      "Stroke": "Medley",
      "DistanceInMeters": 200,
      "SwimTime": {
        "TimeInMs": 151980,
        "DisplayValue": "2:31.98"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 15,
          "Month": 9,
          "Year": 2007
        },
        "City": "Veenendaal"
      }
    },
    {
      "Stroke": "FreestyleLap",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 26410,
        "DisplayValue": "26.41"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "Open Nederlandse Masters Kampioenschappen 2018 lb",
        "Date": {
          "Day": 6,
          "Month": 5,
          "Year": 2018
        },
        "City": "Den\u00A0Haag"
      }
    },
    {
      "Stroke": "FreestyleLap",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 25420,
        "DisplayValue": "25.42"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 5,
          "Month": 10,
          "Year": 2019
        },
        "City": "Westervoort"
      }
    },
    {
      "Stroke": "FreestyleLap",
      "DistanceInMeters": 100,
      "SwimTime": {
        "TimeInMs": 59870,
        "DisplayValue": "59.87"
      },
      "PoolLength": 50,
      "Points": 0,
      "Meet": {
        "Name": "Open Nederlandse Masters Kampioenschappen 2018 lb",
        "Date": {
          "Day": 6,
          "Month": 5,
          "Year": 2018
        },
        "City": "Den\u00A0Haag"
      }
    },
    {
      "Stroke": "ButterflyLap",
      "DistanceInMeters": 50,
      "SwimTime": {
        "TimeInMs": 29790,
        "DisplayValue": "29.79"
      },
      "PoolLength": 25,
      "Points": 0,
      "Meet": {
        "Name": "",
        "Date": {
          "Day": 23,
          "Month": 2,
          "Year": 2019
        },
        "City": "Westervoort"
      }
    }
  ],
  "LastUpdated": "2025-03-18T08:55:31.7154767+01:00",
  "Id": "4046710",
  "FirstName": "Jordi",
  "LastName": "Jolink"
}
```

### Fetch Club Data

```csharp
var clubId = "1234"; // Replace with actual club ID
var httpClient = new HttpClient();
var clubApi = new ClubApi(httpClient);

var club = await clubApi.GetAsync(clubId);
Console.WriteLine($"Club: {club.Name} ({club.Country})");
Console.WriteLine($"Total Members: {club.Members.Count}");
```

### Filter Swimmers by Performance

```csharp
using SwimRankings.Api.Helpers;

// Get all swimmers who can swim 50m Freestyle under 30 seconds
var qualifiedSwimmers = swimmers.FilterByMinimumTime(
    stroke: Stroke.Freestyle,
    distanceInMeters: 50,
    maxTimeInMs: 30000,
    poolLength: 25
);
```

### Find Best Relay Team

```csharp
using SwimRankings.Api.Helpers;

// Find the best 4x50m Freestyle relay team
var relayTeam = RelayTeamHelper.FindBestRelayTeam(
    swimmers: swimmers,
    stroke: Stroke.Freestyle,
    distanceInMeters: 50,
    poolLength: 25,
    gender: Gender.Male
);

Console.WriteLine($"Best Relay Team (Total: {relayTeam.GetDisplayTime()}):");
foreach (var member in relayTeam.Members)
{
    Console.WriteLine($"Position {member.Position}: {member.Swimmer.FirstName} {member.Swimmer.LastName}");
}
```

### Find Best Medley Relay Team

```csharp
// Find the best medley relay (Backstroke, Breaststroke, Butterfly, Freestyle)
var medleyTeam = RelayTeamHelper.FindBestMedleyRelayTeam(
    swimmers: swimmers,
    distanceInMeters: 100,
    poolLength: 50,
    gender: Gender.Female
);
```

## n8n Integration

This library is designed to work seamlessly with n8n workflows for automation of swimming club management tasks. See [N8N_INTEGRATION_GUIDE.md](SwimRankings/N8N_INTEGRATION_GUIDE.md) for detailed instructions on:

- Setting up workflows to fetch and manage swimmer data
- Automating competition roster selection
- Optimizing relay team compositions
- Tracking performance improvements
- Exporting data for analysis

## Use Cases

- **Club Management**: Fetch and manage data for all swimmers in your club
- **Competition Selection**: Automatically select swimmers who meet qualifying times
- **Relay Optimization**: Calculate the fastest relay team combinations
- **Performance Tracking**: Monitor swimmer improvements over time
- **Workflow Automation**: Integrate with n8n for automated reporting and notifications

## Examples

See the `SwimRankings.Api.Examples` project for complete examples including:
- Fetching club data
- Filtering swimmers by criteria
- Optimizing relay teams
- Exporting data for n8n workflows

## API Reference

### ClubApi
- `GetAsync(clubId)` - Fetch club data and member list

### SwimmerApi
- `GetAsync(swimmerId)` - Fetch detailed swimmer data with personal bests

### Filter Helpers
- `FilterByMinimumTime()` - Filter swimmers by performance criteria
- `FilterByGender()` - Filter swimmers by gender
- `FilterByYearOfBirth()` - Filter swimmers by age range

### Relay Team Helpers
- `FindBestRelayTeam()` - Find optimal relay team for an event
- `FindBestMedleyRelayTeam()` - Find optimal medley relay team
- `GetBestTime()` - Get a swimmer's best time for an event