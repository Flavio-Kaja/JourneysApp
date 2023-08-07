namespace JourneyService.FunctionalTests.FunctionalTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class GetJourneyTests : TestBase
{
    [Fact]
    public async Task get_journey_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeJourney = new FakeJourneyBuilder().Build();
        await InsertAsync(fakeJourney);

        // Act
        var route = ApiRoutes.Journeys.GetRecord(fakeJourney.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}