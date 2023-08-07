namespace JourneyService.UnitTests.Domain.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.Domain.Journeys;
using JourneyService.Domain.Journeys.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

public class CreateJourneyTests
{
    private readonly Faker _faker;

    public CreateJourneyTests()
    {
        _faker = new Faker();
    }

    [Fact]
    public void can_create_valid_journey()
    {
        // Arrange
        var journeyToCreate = new FakeJourneyForCreation().Generate();

        // Act
        var fakeJourney = Journey.Create(journeyToCreate);

        // Assert
        fakeJourney.UserId.Should().Be(journeyToCreate.UserId);
        fakeJourney.StartingTime.Should().BeCloseTo(journeyToCreate.StartingTime, 1.Seconds());
        fakeJourney.ArrivalTime.Should().BeCloseTo(journeyToCreate.ArrivalTime.Value, 1.Seconds());
        fakeJourney.TransportationTypeId.Should().Be(journeyToCreate.TransportationTypeId);
        fakeJourney.RouteDistance.Should().Be(journeyToCreate.RouteDistance);
    }

    [Fact]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var journeyToCreate = new FakeJourneyForCreation().Generate();

        // Act
        var fakeJourney = Journey.Create(journeyToCreate);

        // Assert
        fakeJourney.DomainEvents.Count.Should().Be(1);
        fakeJourney.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(JourneyCreated));
    }
}