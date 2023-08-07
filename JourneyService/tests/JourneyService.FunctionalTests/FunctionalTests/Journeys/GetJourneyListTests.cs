namespace JourneyService.FunctionalTests.FunctionalTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class GetJourneyListTests : TestBase
{
    [Fact]
    public async Task get_journey_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.Journeys.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}