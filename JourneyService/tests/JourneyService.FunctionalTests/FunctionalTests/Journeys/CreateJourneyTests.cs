namespace JourneyService.FunctionalTests.FunctionalTests.Journeys;

using JourneyService.SharedTestHelpers.Fakes.Journey;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class CreateJourneyTests : TestBase
{
    [Fact]
    public async Task create_journey_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeJourney = new FakeJourneyForCreationDto().Generate();

        // Act
        var route = ApiRoutes.Journeys.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeJourney);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}