using NUnit.Framework;
using SwimRankings.Api.Helpers;
using SwimRankings.Api.Models;

namespace SwimRankings.Api.Tests;

[TestFixture]
public class SwimmerFilterHelperTests
{
    private List<SwimmerData> _swimmers = null!;

    [SetUp]
    public void Setup()
    {
        _swimmers = new List<SwimmerData>
        {
            new SwimmerData("1")
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                YearOfBirth = 2005,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 25000, DisplayValue = "25.00" }
                    },
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 100,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 55000, DisplayValue = "55.00" }
                    }
                }
            },
            new SwimmerData("2")
            {
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Gender.Female,
                YearOfBirth = 2006,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 27000, DisplayValue = "27.00" }
                    },
                    new()
                    {
                        Stroke = Stroke.Backstroke,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 32000, DisplayValue = "32.00" }
                    }
                }
            },
            new SwimmerData("3")
            {
                FirstName = "Bob",
                LastName = "Johnson",
                Gender = Gender.Male,
                YearOfBirth = 2007,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 31000, DisplayValue = "31.00" }
                    }
                }
            }
        };
    }

    [Test]
    public void FilterByMinimumTime_ReturnsSwimmersUnderThreshold()
    {
        var result = _swimmers.FilterByMinimumTime(
            Stroke.Freestyle,
            50,
            26000,
            25
        );

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].FirstName, Is.EqualTo("John"));
    }

    [Test]
    public void FilterByMinimumTime_IncludesSwimmersAtThreshold()
    {
        var result = _swimmers.FilterByMinimumTime(
            Stroke.Freestyle,
            50,
            27000,
            25
        );

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public void FilterByMinimumTime_WithoutPoolLength_ConsidersBothPools()
    {
        const int maxTimeForAllThreeSwimmers = 32000;
        
        var result = _swimmers.FilterByMinimumTime(
            Stroke.Freestyle,
            50,
            maxTimeForAllThreeSwimmers,
            null
        );

        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetBestTime_ReturnsMinimumTime()
    {
        var bestTime = _swimmers[0].GetBestTime(Stroke.Freestyle, 50, 25);

        Assert.That(bestTime, Is.EqualTo(25000));
    }

    [Test]
    public void GetBestTime_ReturnsNullWhenNoMatchingPb()
    {
        var bestTime = _swimmers[0].GetBestTime(Stroke.Butterfly, 50, 25);

        Assert.That(bestTime, Is.Null);
    }

    [Test]
    public void FilterByGender_ReturnsMaleSwimmers()
    {
        var result = _swimmers.FilterByGender(Gender.Male);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.All(s => s.Gender == Gender.Male), Is.True);
    }

    [Test]
    public void FilterByGender_ReturnsFemaleSwimmers()
    {
        var result = _swimmers.FilterByGender(Gender.Female);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].FirstName, Is.EqualTo("Jane"));
    }

    [Test]
    public void FilterByYearOfBirth_ReturnsSwimmersInRange()
    {
        var result = _swimmers.FilterByYearOfBirth(2005, 2006);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.Any(s => s.YearOfBirth == 2007), Is.False);
    }

    [Test]
    public void FilterByYearOfBirth_IncludesBoundaries()
    {
        var result = _swimmers.FilterByYearOfBirth(2005, 2005);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].YearOfBirth, Is.EqualTo(2005));
    }
}
