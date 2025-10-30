# n8n Integration Guide for SwimRankings

This guide explains how to use the SwimRankings API in n8n workflows to manage swimmer data from a swimming club.

## Overview

The SwimRankings API now supports:
- Fetching all swimmers from a specific club
- Filtering swimmers by performance criteria (minimum times)
- Finding optimal relay team compositions
- Exporting data in JSON format for workflow automation

## Use Cases

1. **Fetch Club Swimmers**: Get all members of a swimming club
2. **Filter by Performance**: Select swimmers who meet specific time standards
3. **Relay Team Optimization**: Automatically find the best relay team combinations
4. **Competition Selection**: Identify swimmers eligible for competitions based on qualifying times
5. **Data Management**: Export and manipulate swimmer data for reporting

## API Methods

### ClubApi

#### GetAsync(clubId)
Retrieves club information and all members.

**Example:**
```csharp
var clubApi = new ClubApi(httpClient);
var club = await clubApi.GetAsync("1234");
```

**Returns:**
```json
{
  "Id": "1234",
  "Name": "Swimming Club Name",
  "Country": "Country",
  "Members": [
    {
      "Id": "4046710",
      "FirstName": "John",
      "LastName": "Doe",
      "Gender": "Male",
      "YearOfBirth": 2005
    }
  ]
}
```

### SwimmerApi

#### GetAsync(swimmerId)
Retrieves detailed data including personal bests for a specific swimmer.

**Example:**
```csharp
var swimmerApi = new SwimmerApi(httpClient);
var swimmer = await swimmerApi.GetAsync("4046710");
```

### Filter Helpers

#### FilterByMinimumTime
Filter swimmers who meet or exceed a specific time standard.

**Example:**
```csharp
var qualified = swimmers.FilterByMinimumTime(
    stroke: Stroke.Freestyle,
    distanceInMeters: 100,
    maxTimeInMs: 60000, // 1:00.00
    poolLength: 50
);
```

#### FilterByGender
Filter swimmers by gender.

**Example:**
```csharp
var maleSwimmers = swimmers.FilterByGender(Gender.Male);
```

#### FilterByYearOfBirth
Filter swimmers by age/year range.

**Example:**
```csharp
var juniorSwimmers = swimmers.FilterByYearOfBirth(2005, 2010);
```

### Relay Team Optimization

#### FindBestRelayTeam
Find the fastest 4-person relay team for a specific event.

**Example:**
```csharp
var relayTeam = RelayTeamHelper.FindBestRelayTeam(
    swimmers: swimmers,
    stroke: Stroke.Freestyle,
    distanceInMeters: 50,
    poolLength: 25,
    gender: Gender.Male
);
```

#### FindBestMedleyRelayTeam
Find the fastest medley relay team (Backstroke, Breaststroke, Butterfly, Freestyle).

**Example:**
```csharp
var medleyTeam = RelayTeamHelper.FindBestMedleyRelayTeam(
    swimmers: swimmers,
    distanceInMeters: 100,
    poolLength: 50,
    gender: Gender.Female
);
```

## n8n Workflow Examples

### Basic Workflow: Fetch Club Data

1. **HTTP Request Node** (Get Club Members)
   - Method: Use the ClubApi in a custom C# function
   - URL: Execute via webhook or scheduled trigger
   - Output: List of club members

2. **Split In Batches Node**
   - Process members in batches to fetch detailed data

3. **HTTP Request Node** (Get Swimmer Details)
   - For each member, fetch full swimmer data with personal bests

4. **Function Node** (Filter and Process)
   - Apply filters based on your criteria
   - Calculate relay teams
   - Format data for output

5. **Set Node** or **Database Node**
   - Store processed data
   - Update your club management system

### Advanced Workflow: Competition Selection

1. **Trigger Node** (Schedule or Manual)
   - Run before competitions

2. **Execute Code Node** (Fetch Club Data)
   ```javascript
   // This would call your C# API
   const clubId = "1234";
   // Fetch club data
   ```

3. **Function Node** (Apply Criteria)
   ```javascript
   // Filter swimmers by minimum times
   const qualified = items.filter(swimmer => {
     const freestyle100 = swimmer.json.Pbs.find(
       pb => pb.Stroke === "Freestyle" && 
             pb.DistanceInMeters === 100 &&
             pb.PoolLength === 50
     );
     return freestyle100 && freestyle100.TimeInMs <= 65000; // 1:05.00
   });
   return qualified;
   ```

4. **Function Node** (Calculate Relay Teams)
   ```javascript
   // Use the relay optimization logic
   // Find best 4x100 freestyle relay
   ```

5. **Email or Notification Node**
   - Send list of qualified swimmers and relay teams to coaches

### Workflow: Automated Reporting

1. **Schedule Trigger** (Weekly/Monthly)

2. **Execute Code Node** (Data Collection)
   - Fetch all club swimmers
   - Get latest personal bests

3. **Function Node** (Analysis)
   - Identify improvements
   - Calculate statistics
   - Find potential relay teams

4. **Google Sheets or Database Node**
   - Update tracking spreadsheet
   - Store historical data

5. **Email Node**
   - Send summary report to coaching staff

## Data Export Format

The API provides JSON-formatted data ideal for n8n workflows:

```json
{
  "Club": {
    "Id": "1234",
    "Name": "Swimming Club",
    "Country": "Netherlands",
    "MemberCount": 50
  },
  "Swimmers": [
    {
      "Id": "4046710",
      "FirstName": "John",
      "LastName": "Doe",
      "Gender": "Male",
      "YearOfBirth": 2005,
      "PersonalBests": [
        {
          "Stroke": "Freestyle",
          "DistanceInMeters": 50,
          "PoolLength": 25,
          "TimeInMs": 25790,
          "TimeDisplay": "25.79"
        }
      ]
    }
  ],
  "RelayTeams": {
    "FreestyleRelay": {
      "TotalTime": "1:45.50",
      "Members": [
        {
          "Position": 1,
          "Name": "John Doe",
          "TimeInMs": 26000,
          "TimeDisplay": "26.00"
        }
      ]
    }
  }
}
```

## Implementation Steps

### 1. Create a Web Service
Deploy the SwimRankings API as a web service (e.g., using ASP.NET Core Web API) that exposes REST endpoints:

```
GET /api/club/{clubId}
GET /api/swimmer/{swimmerId}
POST /api/swimmers/filter
POST /api/relay/optimize
```

### 2. Configure n8n HTTP Request Nodes
Point your n8n HTTP Request nodes to your deployed API endpoints.

### 3. Process and Transform Data
Use n8n's built-in nodes to filter, transform, and route data based on your needs.

### 4. Integrate with Other Systems
Connect to email, databases, spreadsheets, or other tools to complete your workflow.

## Tips for n8n Integration

1. **Batch Processing**: When fetching detailed data for many swimmers, use batch processing to avoid overwhelming the SwimRankings.net server
2. **Caching**: Store club and swimmer data locally to reduce API calls
3. **Error Handling**: Implement proper error handling for failed HTTP requests
4. **Rate Limiting**: Be respectful of the SwimRankings.net server by implementing appropriate delays
5. **Data Validation**: Validate data before using it in calculations or decisions

## Example Scenarios

### Scenario 1: Weekly Performance Tracking
Track club members' performances and notify coaches of improvements.

### Scenario 2: Competition Roster Selection
Automatically select swimmers who meet qualifying times for upcoming competitions.

### Scenario 3: Relay Team Planning
Calculate and suggest optimal relay team compositions for different events.

### Scenario 4: Age Group Analysis
Analyze performance trends across different age groups in your club.

## Support

For issues or questions:
1. Check the API documentation in the code
2. Review the example implementations
3. Open an issue on the GitHub repository
