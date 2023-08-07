namespace JourneyService.IntegrationTests.FeatureTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.Domain.Journeys.Features;
using Domain;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using JourneyService.Exceptions;

public class JourneyQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_journey_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeJourneyOne = new FakeJourneyBuilder().Build();
        await testingServiceScope.InsertAsync(fakeJourneyOne);

        // Act
        var query = new GetJourney.Query(fakeJourneyOne.Id);
        var journey = await testingServiceScope.SendAsync(query);

        // Assert
        journey.UserId.Should().Be(fakeJourneyOne.UserId);
        journey.StartingLocationId.Should().Be(fakeJourneyOne.StartingLocationId);
        journey.ArrivalLocationId.Should().Be(fakeJourneyOne.ArrivalLocationId);
        journey.StartingTime.Should().BeCloseTo(fakeJourneyOne.StartingTime, 1.Seconds());
        journey.ArrivalTime.Should().BeCloseTo(fakeJourneyOne.ArrivalTime.Value, 1.Seconds());
        journey.TransportationTypeId.Should().Be(fakeJourneyOne.TransportationTypeId);
        journey.RouteDistance.Should().Be(fakeJourneyOne.RouteDistance);
        journey.IsGoalAchieved.Should().Be(fakeJourneyOne.IsGoalAchieved);
    }

    [Fact]
    public async Task get_journey_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetJourney.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}