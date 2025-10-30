namespace SwimRankings.Api.Helpers;

public static class SwimrankingsUrlHelper
{
    public static string Get(string swimrankingsId, string language = "us") =>
        $"https://www.swimrankings.net/index.php?page=athleteDetail&athleteId={swimrankingsId}&language=us";
    
    public static string GetClubUrl(string clubId, string language = "us") =>
        $"https://www.swimrankings.net/index.php?page=clubDetail&clubId={clubId}&language=us";
}