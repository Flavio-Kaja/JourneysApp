namespace JourneyService.FunctionalTests.FunctionalTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class GetTransportationTypeListTests : TestBase
{
    [Fact]
    public async Task get_transportationtype_list_returns_success()
    {
        // Arrange
        

        // Act
        var result = await FactoryClient.GetRequestAsync(ApiRoutes.TransportationTypes.GetList);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}