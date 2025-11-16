using SwimRankings.Api.Models;

namespace SwimRankings.Api;

public interface IClubApi
{
    /// <summary>
    /// Retrieves club data including all members for a specified SwimRankings club ID.
    /// </summary>
    /// <param name="clubId">The SwimRankings ID of the club to retrieve data for.</param>
    /// <returns>The <see cref="Club"/> object populated with club details and member list.</returns>
    Task<Club> GetAsync(string clubId);
}
