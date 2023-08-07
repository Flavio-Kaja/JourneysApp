namespace JourneyService.IntegrationTests.FeatureTests.TransportationTypes;

using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.Domain.TransportationTypes.Features;
using FluentAssertions;
using Domain;
using Xunit;
using System.Threading.Tasks;

public class TransportationTypeListQueryTests : TestBase
{

    [Fact]
    public async Task can_get_transportationtype_list()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeTransportationTypeOne = new FakeTransportationTypeBuilder().Build();
        var fakeTransportationTypeTwo = new FakeTransportationTypeBuilder().Build();

        await testingServiceScope.InsertAsync(fakeTransportationTypeOne, fakeTransportationTypeTwo);

        // Act
        var query = new GetTransportationTypeList.Query();
        var transportationTypes = await testingServiceScope.SendAsync(query);

        // Assert
        transportationTypes.Count().Should().BeGreaterThanOrEqualTo(2);
    }
}