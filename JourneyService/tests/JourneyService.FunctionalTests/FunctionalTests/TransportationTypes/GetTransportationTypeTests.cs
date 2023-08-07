namespace JourneyService.FunctionalTests.FunctionalTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.FunctionalTests.TestUtilities;
using FluentAssertions;
using Xunit;
using System.Net;
using System.Threading.Tasks;

public class GetTransportationTypeTests : TestBase
{
    [Fact]
    public async Task get_transportationtype_returns_success_when_entity_exists()
    {
        // Arrange
        var fakeTransportationType = new FakeTransportationTypeBuilder().Build();
        await InsertAsync(fakeTransportationType);

        // Act
        var route = ApiRoutes.TransportationTypes.GetRecord(fakeTransportationType.Id);
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}