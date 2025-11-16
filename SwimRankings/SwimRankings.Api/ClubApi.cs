using SwimRankings.Api.Helpers;
using SwimRankings.Api.Models;
using SwimRankings.Api.Services;

namespace SwimRankings.Api;

public class ClubApi(HttpClient httpClient) : IClubApi
{
    /// <summary>
    /// Retrieves club data including all members for a specified SwimRankings club ID.
    /// </summary>
    /// <param name="clubId">The SwimRankings ID of the club to retrieve data for.</param>
    /// <returns>The <see cref="Club"/> object populated with club details and member list.</returns>
    public async Task<Club> GetAsync(string clubId)
    {
        var pageContents = await GetClubPageContentsAsync(clubId);
        var club = new Club
        {
            Id = clubId
        };
        
        club = club
            .WithClubDetails(pageContents)
            .WithMembers(pageContents);

        return club;
    }
    
    private Task<string> GetClubPageContentsAsync(string clubId) =>
        httpClient.GetStringAsync(SwimrankingsUrlHelper.GetClubUrl(clubId));
}
