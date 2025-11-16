using SwimRankings.Api.Models;

namespace SwimRankings.Api.Helpers;

/// <summary>
/// Helper class for filtering and selecting swimmers based on performance criteria.
/// </summary>
public static class SwimmerFilterHelper
{
    /// <summary>
    /// Filters swimmers who have achieved a specific time or better in a given event.
    /// </summary>
    /// <param name="swimmers">List of swimmers with their data.</param>
    /// <param name="stroke">The stroke to filter by.</param>
    /// <param name="distanceInMeters">The distance in meters.</param>
    /// <param name="maxTimeInMs">Maximum time in milliseconds (swimmers must be equal or faster).</param>
    /// <param name="poolLength">Optional pool length (25 or 50). If null, considers both.</param>
    /// <returns>List of swimmers meeting the criteria.</returns>
    public static List<SwimmerData> FilterByMinimumTime(
        this IEnumerable<SwimmerData> swimmers,
        Stroke stroke,
        int distanceInMeters,
        int maxTimeInMs,
        int? poolLength = null)
    {
        return swimmers
            .Where(swimmer => swimmer.Pbs.Any(pb =>
                pb.Stroke == stroke &&
                pb.DistanceInMeters == distanceInMeters &&
                pb.SwimTime.TimeInMs > 0 &&
                pb.SwimTime.TimeInMs <= maxTimeInMs &&
                (poolLength == null || pb.PoolLength == poolLength)))
            .ToList();
    }

    /// <summary>
    /// Gets the best time for a swimmer in a specific event.
    /// </summary>
    /// <param name="swimmer">The swimmer data.</param>
    /// <param name="stroke">The stroke.</param>
    /// <param name="distanceInMeters">The distance in meters.</param>
    /// <param name="poolLength">Optional pool length (25 or 50). If null, considers both.</param>
    /// <returns>Best time in milliseconds, or null if no time found.</returns>
    public static int? GetBestTime(
        this SwimmerData swimmer,
        Stroke stroke,
        int distanceInMeters,
        int? poolLength = null)
    {
        var relevantPbs = swimmer.Pbs
            .Where(pb =>
                pb.Stroke == stroke &&
                pb.DistanceInMeters == distanceInMeters &&
                pb.SwimTime.TimeInMs > 0 &&
                (poolLength == null || pb.PoolLength == poolLength))
            .ToList();

        if (!relevantPbs.Any())
        {
            return null;
        }

        return relevantPbs.Min(pb => pb.SwimTime.TimeInMs);
    }

    /// <summary>
    /// Filters swimmers by gender.
    /// </summary>
    public static List<SwimmerData> FilterByGender(
        this IEnumerable<SwimmerData> swimmers,
        Gender gender)
    {
        return swimmers.Where(s => s.Gender == gender).ToList();
    }

    /// <summary>
    /// Filters swimmers by year of birth range.
    /// </summary>
    public static List<SwimmerData> FilterByYearOfBirth(
        this IEnumerable<SwimmerData> swimmers,
        int minYear,
        int maxYear)
    {
        return swimmers
            .Where(s => s.YearOfBirth >= minYear && s.YearOfBirth <= maxYear)
            .ToList();
    }
}
