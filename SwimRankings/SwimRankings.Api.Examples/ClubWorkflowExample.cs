using System.Text.Json;
using SwimRankings.Api;
using SwimRankings.Api.Helpers;
using SwimRankings.Api.Models;

namespace SwimRankings.Api.Examples;

/// <summary>
/// Example demonstrating how to use the ClubApi for n8n workflows
/// </summary>
public static class ClubWorkflowExample
{
    public static async Task RunAsync()
    {
        // Example: Fetch club data and all swimmers
        var clubId = "1234"; // Replace with actual club ID
        var httpClient = new HttpClient();
        var clubApi = new ClubApi(httpClient);

        Console.WriteLine("=== Fetching Club Data ===");
        var club = await clubApi.GetAsync(clubId);
        Console.WriteLine($"Club: {club.Name} ({club.Country})");
        Console.WriteLine($"Total Members: {club.Members.Count}");
        Console.WriteLine();

        // Example: Fetch detailed data for all club swimmers
        Console.WriteLine("=== Fetching Detailed Swimmer Data ===");
        var swimmerApi = new SwimmerApi(httpClient);
        var swimmers = new List<SwimmerData>();

        foreach (var member in club.Members.Take(5)) // Limit to first 5 for demo
        {
            try
            {
                var swimmerData = await swimmerApi.GetAsync(member.Id);
                swimmers.Add(swimmerData);
                Console.WriteLine($"Loaded: {swimmerData.FirstName} {swimmerData.LastName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading swimmer {member.Id}: {ex.Message}");
            }
        }
        Console.WriteLine();

        // Example: Filter swimmers who meet minimum time criteria
        Console.WriteLine("=== Filtering Swimmers by Minimum Time ===");
        var qualifiedSwimmers = swimmers.FilterByMinimumTime(
            stroke: Stroke.Freestyle,
            distanceInMeters: 50,
            maxTimeInMs: 30000, // 30 seconds
            poolLength: 25
        );
        Console.WriteLine($"Swimmers with 50m Freestyle under 30.00s (25m pool): {qualifiedSwimmers.Count}");
        foreach (var swimmer in qualifiedSwimmers)
        {
            var bestTime = swimmer.GetBestTime(Stroke.Freestyle, 50, 25);
            var timeDisplay = bestTime.HasValue ? $"{bestTime.Value / 1000.0:F2}s" : "N/A";
            Console.WriteLine($"  - {swimmer.FirstName} {swimmer.LastName}: {timeDisplay}");
        }
        Console.WriteLine();

        // Example: Find best freestyle relay team (4x50m)
        Console.WriteLine("=== Finding Best Freestyle Relay Team ===");
        var maleSwimmers = swimmers.FilterByGender(Gender.Male);
        var relayTeam = RelayTeamHelper.FindBestRelayTeam(
            swimmers: maleSwimmers,
            stroke: Stroke.Freestyle,
            distanceInMeters: 50,
            poolLength: 25,
            gender: Gender.Male
        );

        if (relayTeam != null)
        {
            Console.WriteLine($"Best 4x50m Freestyle Relay Team (Total: {relayTeam.GetDisplayTime()}):");
            foreach (var member in relayTeam.Members)
            {
                var timeDisplay = $"{member.TimeInMs / 1000.0:F2}s";
                Console.WriteLine($"  Position {member.Position}: {member.Swimmer.FirstName} {member.Swimmer.LastName} - {timeDisplay}");
            }
        }
        else
        {
            Console.WriteLine("Could not form a complete relay team.");
        }
        Console.WriteLine();

        // Example: Find best medley relay team
        Console.WriteLine("=== Finding Best Medley Relay Team ===");
        var medleyTeam = RelayTeamHelper.FindBestMedleyRelayTeam(
            swimmers: swimmers,
            distanceInMeters: 50,
            poolLength: 25,
            gender: Gender.Male
        );

        if (medleyTeam != null)
        {
            var strokes = new[] { "Backstroke", "Breaststroke", "Butterfly", "Freestyle" };
            Console.WriteLine($"Best 4x50m Medley Relay Team (Total: {medleyTeam.GetDisplayTime()}):");
            foreach (var member in medleyTeam.Members)
            {
                var timeDisplay = $"{member.TimeInMs / 1000.0:F2}s";
                var stroke = strokes[member.Position - 1];
                Console.WriteLine($"  Position {member.Position} ({stroke}): {member.Swimmer.FirstName} {member.Swimmer.LastName} - {timeDisplay}");
            }
        }
        else
        {
            Console.WriteLine("Could not form a complete medley relay team.");
        }
        Console.WriteLine();

        // Example: Export data as JSON for n8n workflow
        Console.WriteLine("=== JSON Export for n8n Workflow ===");
        var exportData = new
        {
            Club = new
            {
                club.Id,
                club.Name,
                club.Country,
                MemberCount = club.Members.Count
            },
            Swimmers = swimmers.Select(s => new
            {
                s.Id,
                s.FirstName,
                s.LastName,
                s.Gender,
                s.YearOfBirth,
                s.Club,
                PersonalBests = s.Pbs.Select(pb => new
                {
                    Stroke = pb.Stroke.ToString(),
                    pb.DistanceInMeters,
                    pb.PoolLength,
                    TimeInMs = pb.SwimTime.TimeInMs,
                    TimeDisplay = pb.SwimTime.DisplayValue,
                    Meet = new
                    {
                        pb.Meet.Name,
                        pb.Meet.Date,
                        pb.Meet.City
                    }
                }).ToList()
            }).ToList(),
            RelayTeams = new
            {
                FreestyleRelay = relayTeam != null ? new
                {
                    TotalTime = relayTeam.GetDisplayTime(),
                    Members = relayTeam.Members.Select(m => new
                    {
                        Position = m.Position,
                        Name = $"{m.Swimmer.FirstName} {m.Swimmer.LastName}",
                        TimeInMs = m.TimeInMs,
                        TimeDisplay = $"{m.TimeInMs / 1000.0:F2}"
                    }).ToList()
                } : null,
                MedleyRelay = medleyTeam != null ? new
                {
                    TotalTime = medleyTeam.GetDisplayTime(),
                    Members = medleyTeam.Members.Select(m => new
                    {
                        Position = m.Position,
                        Name = $"{m.Swimmer.FirstName} {m.Swimmer.LastName}",
                        TimeInMs = m.TimeInMs,
                        TimeDisplay = $"{m.TimeInMs / 1000.0:F2}"
                    }).ToList()
                } : null
            }
        };

        Console.WriteLine(JsonSerializer.Serialize(exportData, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        }));
    }
}
