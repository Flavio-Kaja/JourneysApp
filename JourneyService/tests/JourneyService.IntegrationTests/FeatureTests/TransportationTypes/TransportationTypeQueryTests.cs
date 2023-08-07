namespace JourneyService.IntegrationTests.FeatureTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.Domain.TransportationTypes.Features;
using Domain;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using JourneyService.Exceptions;

public class TransportationTypeQueryTests : TestBase
{
    [Fact]
    public async Task can_get_existing_transportationtype_with_accurate_props()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeTransportationTypeOne = new FakeTransportationTypeBuilder().Build();
        await testingServiceScope.InsertAsync(fakeTransportationTypeOne);

        // Act
        var query = new GetTransportationType.Query(fakeTransportationTypeOne.Id);
        var transportationType = await testingServiceScope.SendAsync(query);

        // Assert
        transportationType.Type.Should().Be(fakeTransportationTypeOne.Type);
    }

    [Fact]
    public async Task get_transportationtype_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var query = new GetTransportationType.Query(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}