namespace JourneyService.IntegrationTests.FeatureTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.Domain.Journeys.Dtos;
using JourneyService.Domain.Journeys.Features;
using Domain;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;

public class UpdateJourneyCommandTests : TestBase
{
    [Fact]
    public async Task can_update_existing_journey_in_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeJourneyOne = new FakeJourneyBuilder().Build();
        var updatedJourneyDto = new FakeJourneyForUpdate().Generate();
        await testingServiceScope.InsertAsync(fakeJourneyOne);

        var journey = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys
            .FirstOrDefaultAsync(j => j.Id == fakeJourneyOne.Id));

        // Act
        var command = new UpdateJourney.Command(journey.Id, updatedJourneyDto);
        await testingServiceScope.SendAsync(command);
        var updatedJourney = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys.FirstOrDefaultAsync(j => j.Id == journey.Id));

        // Assert
        updatedJourney.UserId.Should().Be(updatedJourneyDto.UserId);
        updatedJourney.StartingTime.Should().BeCloseTo(updatedJourneyDto.StartingTime, 1.Seconds());
        updatedJourney.ArrivalTime.Should().BeCloseTo(updatedJourneyDto.ArrivalTime.Value, 1.Seconds());
        updatedJourney.TransportationTypeId.Should().Be(updatedJourneyDto.TransportationTypeId);
        updatedJourney.RouteDistance.Should().Be(updatedJourneyDto.RouteDistance);
    }
}