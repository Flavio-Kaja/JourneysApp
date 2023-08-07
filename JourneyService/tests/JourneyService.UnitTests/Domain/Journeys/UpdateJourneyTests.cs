namespace JourneyService.UnitTests.Domain.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.Domain.Journeys;
using JourneyService.Domain.Journeys.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

public class UpdateJourneyTests
{
    private readonly Faker _faker;

    public UpdateJourneyTests()
    {
        _faker = new Faker();
    }

    [Fact]
    public void can_update_journey()
    {
        // Arrange
        var fakeJourney = new FakeJourneyBuilder().Build();
        var updatedJourney = new FakeJourneyForUpdate().Generate();

        // Act
        fakeJourney.Update(updatedJourney);

        // Assert
        fakeJourney.UserId.Should().Be(updatedJourney.UserId);
        fakeJourney.StartingTime.Should().BeCloseTo(updatedJourney.StartingTime, 1.Seconds());
        fakeJourney.ArrivalTime.Should().BeCloseTo(updatedJourney.ArrivalTime.Value, 1.Seconds());
        fakeJourney.TransportationTypeId.Should().Be(updatedJourney.TransportationTypeId);
        fakeJourney.RouteDistance.Should().Be(updatedJourney.RouteDistance);
    }

    [Fact]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeJourney = new FakeJourneyBuilder().Build();
        var updatedJourney = new FakeJourneyForUpdate().Generate();
        fakeJourney.DomainEvents.Clear();

        // Act
        fakeJourney.Update(updatedJourney);

        // Assert
        fakeJourney.DomainEvents.Count.Should().Be(1);
        fakeJourney.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(JourneyUpdated));
    }
}