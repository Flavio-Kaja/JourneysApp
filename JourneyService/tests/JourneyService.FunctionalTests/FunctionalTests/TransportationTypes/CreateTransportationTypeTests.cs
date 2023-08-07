namespace JourneyService.FunctionalTests.FunctionalTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class CreateTransportationTypeTests : TestBase
{
    [Fact]
    public async Task create_transportationtype_returns_created_using_valid_dto()
    {
        // Arrange
        var fakeTransportationType = new FakeTransportationTypeForCreationDto().Generate();

        // Act
        var route = ApiRoutes.TransportationTypes.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeTransportationType);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}