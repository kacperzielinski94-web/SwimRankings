namespace SwimRankings.Api.Models;

public class ClubMember : Swimmer
{
    public ClubMember(string id) : base(id)
    {
    }
    
    public Gender Gender { get; set; } = Gender.Unknown;
    
    public int YearOfBirth { get; set; }
}
