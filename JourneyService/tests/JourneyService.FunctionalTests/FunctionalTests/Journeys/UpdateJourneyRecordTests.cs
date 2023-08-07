namespace JourneyService.FunctionalTests.FunctionalTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class UpdateJourneyRecordTests : TestBase
{
    [Fact]
    public async Task put_journey_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeJourney = new FakeJourneyBuilder().Build();
        var updatedJourneyDto = new FakeJourneyForUpdate().Generate();
        await InsertAsync(fakeJourney);

        // Act
        var route = ApiRoutes.Journeys.Put(fakeJourney.Id);
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedJourneyDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}