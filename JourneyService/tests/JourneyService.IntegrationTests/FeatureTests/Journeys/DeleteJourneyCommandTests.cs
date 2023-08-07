namespace JourneyService.IntegrationTests.FeatureTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.Domain.Journeys.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Domain;
using System.Threading.Tasks;
using JourneyService.Exceptions;

public class DeleteJourneyCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_journey_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeJourneyOne = new FakeJourneyBuilder().Build();
        await testingServiceScope.InsertAsync(fakeJourneyOne);
        var journey = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys
            .FirstOrDefaultAsync(j => j.Id == fakeJourneyOne.Id));

        // Act
        var command = new DeleteJourney.Command(journey.Id);
        await testingServiceScope.SendAsync(command);
        var journeyResponse = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys.CountAsync(j => j.Id == journey.Id));

        // Assert
        journeyResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_journey_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteJourney.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_journey_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeJourneyOne = new FakeJourneyBuilder().Build();
        await testingServiceScope.InsertAsync(fakeJourneyOne);
        var journey = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys
            .FirstOrDefaultAsync(j => j.Id == fakeJourneyOne.Id));

        // Act
        var command = new DeleteJourney.Command(journey.Id);
        await testingServiceScope.SendAsync(command);
        var deletedJourney = await testingServiceScope.ExecuteDbContextAsync(db => db.Journeys
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == journey.Id));

        // Assert
        deletedJourney?.IsDeleted.Should().BeTrue();
    }
}