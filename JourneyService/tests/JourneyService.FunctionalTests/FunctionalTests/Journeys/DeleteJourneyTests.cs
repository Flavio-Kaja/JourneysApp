namespace JourneyService.FunctionalTests.FunctionalTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class DeleteJourneyTests : TestBase
{
    [Fact]
    public async Task delete_journey_returns_nocontent_when_entity_exists()
    {
        // Arrange
        var fakeJourney = new FakeJourneyBuilder().Build();
        await InsertAsync(fakeJourney);

        // Act
        var route = ApiRoutes.Journeys.Delete(fakeJourney.Id);
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}