using NUnit.Framework;
using SwimRankings.Api.Helpers;
using SwimRankings.Api.Models;

namespace SwimRankings.Api.Tests;

[TestFixture]
public class RelayTeamHelperTests
{
    private List<SwimmerData> _swimmers = null!;

    [SetUp]
    public void Setup()
    {
        _swimmers = new List<SwimmerData>
        {
            new SwimmerData("1")
            {
                FirstName = "Fast",
                LastName = "Swimmer1",
                Gender = Gender.Male,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 24000, DisplayValue = "24.00" }
                    },
                    new()
                    {
                        Stroke = Stroke.Backstroke,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 30000, DisplayValue = "30.00" }
                    }
                }
            },
            new SwimmerData("2")
            {
                FirstName = "Fast",
                LastName = "Swimmer2",
                Gender = Gender.Male,
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
                        Stroke = Stroke.Breaststroke,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 33000, DisplayValue = "33.00" }
                    }
                }
            },
            new SwimmerData("3")
            {
                FirstName = "Fast",
                LastName = "Swimmer3",
                Gender = Gender.Male,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 26000, DisplayValue = "26.00" }
                    },
                    new()
                    {
                        Stroke = Stroke.Butterfly,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 28000, DisplayValue = "28.00" }
                    }
                }
            },
            new SwimmerData("4")
            {
                FirstName = "Fast",
                LastName = "Swimmer4",
                Gender = Gender.Male,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 27000, DisplayValue = "27.00" }
                    }
                }
            },
            new SwimmerData("5")
            {
                FirstName = "Female",
                LastName = "Swimmer",
                Gender = Gender.Female,
                Pbs = new List<Pb>
                {
                    new()
                    {
                        Stroke = Stroke.Freestyle,
                        DistanceInMeters = 50,
                        PoolLength = 25,
                        SwimTime = new SwimTime { TimeInMs = 26500, DisplayValue = "26.50" }
                    }
                }
            }
        };
    }

    [Test]
    public void FindBestRelayTeam_Returns4FastestSwimmers()
    {
        var relayTeam = RelayTeamHelper.FindBestRelayTeam(
            _swimmers,
            Stroke.Freestyle,
            50,
            25,
            Gender.Male
        );

        Assert.That(relayTeam, Is.Not.Null);
        Assert.That(relayTeam!.Members.Count, Is.EqualTo(4));
        Assert.That(relayTeam.Members[0].TimeInMs, Is.EqualTo(24000));
        Assert.That(relayTeam.Members[1].TimeInMs, Is.EqualTo(25000));
        Assert.That(relayTeam.Members[2].TimeInMs, Is.EqualTo(26000));
        Assert.That(relayTeam.Members[3].TimeInMs, Is.EqualTo(27000));
    }

    [Test]
    public void FindBestRelayTeam_FiltersByGender()
    {
        var relayTeam = RelayTeamHelper.FindBestRelayTeam(
            _swimmers,
            Stroke.Freestyle,
            50,
            25,
            Gender.Female
        );

        Assert.That(relayTeam, Is.Null); // Only 1 female swimmer
    }

    [Test]
    public void FindBestRelayTeam_ReturnsNullWithInsufficientSwimmers()
    {
        var limitedSwimmers = _swimmers.Take(2).ToList();
        var relayTeam = RelayTeamHelper.FindBestRelayTeam(
            limitedSwimmers,
            Stroke.Freestyle,
            50,
            25,
            Gender.Male
        );

        Assert.That(relayTeam, Is.Null);
    }

    [Test]
    public void FindBestRelayTeam_CalculatesTotalTime()
    {
        var relayTeam = RelayTeamHelper.FindBestRelayTeam(
            _swimmers,
            Stroke.Freestyle,
            50,
            25,
            Gender.Male
        );

        var expectedTotal = 24000 + 25000 + 26000 + 27000;
        Assert.That(relayTeam!.TotalTimeInMs, Is.EqualTo(expectedTotal));
    }

    [Test]
    public void FindBestMedleyRelayTeam_ReturnsTeamWithDifferentStrokes()
    {
        var medleyTeam = RelayTeamHelper.FindBestMedleyRelayTeam(
            _swimmers,
            50,
            25,
            Gender.Male
        );

        Assert.That(medleyTeam, Is.Not.Null);
        Assert.That(medleyTeam!.Members.Count, Is.EqualTo(4));
        
        // Verify different swimmers for different strokes
        var swimmerIds = medleyTeam.Members.Select(m => m.Swimmer.Id).ToList();
        Assert.That(swimmerIds.Distinct().Count(), Is.EqualTo(4));
    }

    [Test]
    public void FindBestMedleyRelayTeam_ReturnsNullWhenMissingStroke()
    {
        var limitedSwimmers = _swimmers.Take(2).ToList();
        var medleyTeam = RelayTeamHelper.FindBestMedleyRelayTeam(
            limitedSwimmers,
            50,
            25,
            Gender.Male
        );

        Assert.That(medleyTeam, Is.Null);
    }

    [Test]
    public void RelayTeam_DisplayTime_FormatsCorrectly()
    {
        var team = new RelayTeamHelper.RelayTeam
        {
            Members = new List<RelayTeamHelper.RelayMember>
            {
                new() { TimeInMs = 25000 },
                new() { TimeInMs = 25000 },
                new() { TimeInMs = 25000 },
                new() { TimeInMs = 25000 }
            }
        };

        var displayTime = team.GetDisplayTime();
        Assert.That(displayTime, Is.EqualTo("1:40.00"));
    }

    [Test]
    public void RelayTeam_DisplayTime_FormatsSecondsOnly()
    {
        var team = new RelayTeamHelper.RelayTeam
        {
            Members = new List<RelayTeamHelper.RelayMember>
            {
                new() { TimeInMs = 12500 },
                new() { TimeInMs = 12500 },
                new() { TimeInMs = 12500 },
                new() { TimeInMs = 12500 }
            }
        };

        var displayTime = team.GetDisplayTime();
        Assert.That(displayTime, Is.EqualTo("50.00"));
    }

    [Test]
    public void FindBestRelayTeam_WithoutGenderFilter_ConsidersAll()
    {
        var relayTeam = RelayTeamHelper.FindBestRelayTeam(
            _swimmers,
            Stroke.Freestyle,
            50,
            25,
            null
        );

        Assert.That(relayTeam, Is.Not.Null);
        Assert.That(relayTeam!.Members.Count, Is.EqualTo(4));
        // Should include the fastest 4, which may include the female swimmer
    }
}
