namespace JourneyService.IntegrationTests.FeatureTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using Domain;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using JourneyService.Domain.Journeys.Features;

public class AddJourneyCommandTests : TestBase
{
    [Fact]
    public async Task can_add_new_journey_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeJourneyOne = new FakeJourneyForCreationDto().Generate();

        // Act
        var command = new AddJourney.Command(fakeJourneyOne);
        var journeyReturned = await testingServiceScope.SendAsync(command);
        var journeyCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys
            .FirstOrDefaultAsync(j => j.Id == journeyReturned.Id));

        // Assert
        journeyReturned.UserId.Should().Be(fakeJourneyOne.UserId);
        journeyReturned.StartingTime.Should().BeCloseTo(fakeJourneyOne.StartingTime, 1.Seconds());
        journeyReturned.ArrivalTime.Should().BeCloseTo(fakeJourneyOne.ArrivalTime.Value, 1.Seconds());
        journeyReturned.TransportationTypeId.Should().Be(fakeJourneyOne.TransportationTypeId);
        journeyReturned.RouteDistance.Should().Be(fakeJourneyOne.RouteDistance);

        journeyCreated.UserId.Should().Be(fakeJourneyOne.UserId);
        journeyCreated.StartingTime.Should().BeCloseTo(fakeJourneyOne.StartingTime, 1.Seconds());
        journeyCreated.ArrivalTime.Should().BeCloseTo(fakeJourneyOne.ArrivalTime.Value, 1.Seconds());
        journeyCreated.TransportationTypeId.Should().Be(fakeJourneyOne.TransportationTypeId);
        journeyCreated.RouteDistance.Should().Be(fakeJourneyOne.RouteDistance);
    }
}