using SwimRankings.Api.Models;

namespace SwimRankings.Api.Helpers;

/// <summary>
/// Helper class for optimizing relay team selection.
/// </summary>
public static class RelayTeamHelper
{
    /// <summary>
    /// Represents a relay team member with their swim time.
    /// </summary>
    public class RelayMember
    {
        public SwimmerData Swimmer { get; set; } = new SwimmerData(string.Empty);
        public int TimeInMs { get; set; }
        public int Position { get; set; }
    }

    /// <summary>
    /// Represents a complete relay team.
    /// </summary>
    public class RelayTeam
    {
        public List<RelayMember> Members { get; set; } = new();
        public int TotalTimeInMs => Members.Sum(m => m.TimeInMs);
        
        public string GetDisplayTime()
        {
            var totalSeconds = TotalTimeInMs / 1000.0;
            var minutes = (int)(totalSeconds / 60);
            var seconds = totalSeconds % 60;
            
            if (minutes > 0)
            {
                return $"{minutes}:{seconds:00.00}";
            }
            return $"{seconds:0.00}";
        }
    }

    /// <summary>
    /// Finds the best relay team (4 swimmers) for a specific event.
    /// </summary>
    /// <param name="swimmers">Available swimmers.</param>
    /// <param name="stroke">The stroke for the relay.</param>
    /// <param name="distanceInMeters">Distance per swimmer in meters.</param>
    /// <param name="poolLength">Pool length (25 or 50).</param>
    /// <param name="gender">Optional gender filter.</param>
    /// <returns>The optimal relay team, or null if insufficient swimmers.</returns>
    public static RelayTeam? FindBestRelayTeam(
        IEnumerable<SwimmerData> swimmers,
        Stroke stroke,
        int distanceInMeters,
        int poolLength,
        Gender? gender = null)
    {
        var eligibleSwimmers = swimmers
            .Where(s => gender == null || s.Gender == gender)
            .Select(s => new
            {
                Swimmer = s,
                BestTime = s.GetBestTime(stroke, distanceInMeters, poolLength)
            })
            .Where(x => x.BestTime.HasValue)
            .OrderBy(x => x.BestTime!.Value)
            .Take(4)
            .ToList();

        if (eligibleSwimmers.Count < 4)
        {
            return null;
        }

        var team = new RelayTeam
        {
            Members = eligibleSwimmers.Select((x, index) => new RelayMember
            {
                Swimmer = x.Swimmer,
                TimeInMs = x.BestTime!.Value,
                Position = index + 1
            }).ToList()
        };

        return team;
    }

    /// <summary>
    /// Finds the best medley relay team (4 swimmers, each swimming a different stroke).
    /// Medley order: Backstroke, Breaststroke, Butterfly, Freestyle
    /// </summary>
    /// <param name="swimmers">Available swimmers.</param>
    /// <param name="distanceInMeters">Distance per swimmer in meters (typically 50 or 100).</param>
    /// <param name="poolLength">Pool length (25 or 50).</param>
    /// <param name="gender">Optional gender filter.</param>
    /// <returns>The optimal medley relay team, or null if insufficient swimmers.</returns>
    public static RelayTeam? FindBestMedleyRelayTeam(
        IEnumerable<SwimmerData> swimmers,
        int distanceInMeters,
        int poolLength,
        Gender? gender = null)
    {
        var swimmerList = swimmers
            .Where(s => gender == null || s.Gender == gender)
            .ToList();

        var strokes = new[] 
        { 
            Stroke.Backstroke, 
            Stroke.Breaststroke, 
            Stroke.Butterfly, 
            Stroke.Freestyle 
        };

        var team = new RelayTeam();
        var usedSwimmers = new HashSet<string>();

        for (int position = 0; position < 4; position++)
        {
            var stroke = strokes[position];
            
            var bestSwimmer = swimmerList
                .Where(s => !usedSwimmers.Contains(s.Id))
                .Select(s => new
                {
                    Swimmer = s,
                    BestTime = s.GetBestTime(stroke, distanceInMeters, poolLength)
                })
                .Where(x => x.BestTime.HasValue)
                .OrderBy(x => x.BestTime!.Value)
                .FirstOrDefault();

            if (bestSwimmer == null)
            {
                return null; // Can't fill all positions
            }

            team.Members.Add(new RelayMember
            {
                Swimmer = bestSwimmer.Swimmer,
                TimeInMs = bestSwimmer.BestTime!.Value,
                Position = position + 1
            });

            usedSwimmers.Add(bestSwimmer.Swimmer.Id);
        }

        return team;
    }
}
