namespace SwimRankings.Api.Models;

public class Club
{
    public string Id { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Country { get; set; } = string.Empty;
    
    public List<ClubMember> Members { get; set; } = new();
    
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
