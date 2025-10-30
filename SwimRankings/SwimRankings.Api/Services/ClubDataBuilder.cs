using System.Net;
using System.Text.RegularExpressions;
using SwimRankings.Api.Extensions;
using SwimRankings.Api.Helpers;
using SwimRankings.Api.Models;

namespace SwimRankings.Api.Services;

internal static class ClubDataBuilder
{
    public static Club WithClubDetails(this Club club, string pageContents)
    {
        var clubName = RegexHelper.GetMatchValue(pageContents, @"<div id=""name"">(.*?)</div>");
        var country = RegexHelper.GetMatchValue(pageContents, @"<div id=""nation"">(.*?)</div>");
        
        club.Name = WebUtility.HtmlDecode(clubName);
        club.Country = WebUtility.HtmlDecode(country);
        
        return club;
    }
    
    public static Club WithMembers(this Club club, string pageContents)
    {
        club.Members = new List<ClubMember>();
        
        // Extract member table
        var memberTable = RegexHelper.GetMatchValue(
            pageContents,
            @"<table class=""athleteList""(.*?)</table>");
        
        if (string.IsNullOrEmpty(memberTable))
        {
            return club;
        }
        
        // Match all member rows
        var memberRows = Regex.Matches(memberTable, @"<tr class=""athleteList(.*?)</tr>");
        
        foreach (Match rowMatch in memberRows)
        {
            var rowContent = rowMatch.Groups[1].Value.Trim();
            var member = CreateMemberFromRow(rowContent);
            
            if (member != null && !string.IsNullOrEmpty(member.Id))
            {
                club.Members.Add(member);
            }
        }
        
        return club;
    }
    
    private static ClubMember? CreateMemberFromRow(string rowContent)
    {
        try
        {
            // Extract athlete ID from the link
            var athleteIdMatch = Regex.Match(rowContent, @"athleteId=(\d+)");
            if (!athleteIdMatch.Success)
            {
                return null;
            }
            
            var athleteId = athleteIdMatch.Groups[1].Value;
            
            // Extract name
            var nameMatch = RegexHelper.GetMatchValue(rowContent, @"<a.*?>(.*?)</a>");
            var nameParts = nameMatch.Split(',');
            
            var lastName = nameParts.Length > 0 ? nameParts[0].Trim() : "";
            var firstName = nameParts.Length > 1 ? nameParts[1].Trim() : "";
            
            // Extract year of birth
            var yearOfBirth = int.Parse(RegexHelper.GetMatchValue(rowContent, @"<td class=""yob"".*?>(.*?)</td>", "0"));
            
            // Extract gender from image
            var gender = Gender.Unknown;
            if (rowContent.Contains("images/gender1.png"))
            {
                gender = Gender.Male;
            }
            else if (rowContent.Contains("images/gender2.png"))
            {
                gender = Gender.Female;
            }
            
            return new ClubMember(athleteId)
            {
                FirstName = firstName.ToNameCasing(),
                LastName = lastName.ToNameCasing(),
                YearOfBirth = yearOfBirth,
                Gender = gender
            };
        }
        catch
        {
            return null;
        }
    }
}
