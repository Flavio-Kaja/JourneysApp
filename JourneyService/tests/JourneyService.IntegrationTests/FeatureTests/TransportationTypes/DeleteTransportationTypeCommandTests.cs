namespace JourneyService.IntegrationTests.FeatureTests.TransportationTypes;

using JourneyService.SharedTestHelpers.Fakes.TransportationType;
using JourneyService.Domain.TransportationTypes.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Domain;
using System.Threading.Tasks;
using JourneyService.Exceptions;

public class DeleteTransportationTypeCommandTests : TestBase
{
    [Fact]
    public async Task can_delete_transportationtype_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeTransportationTypeOne = new FakeTransportationTypeBuilder().Build();
        await testingServiceScope.InsertAsync(fakeTransportationTypeOne);
        var transportationType = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes
            .FirstOrDefaultAsync(t => t.Id == fakeTransportationTypeOne.Id));

        // Act
        var command = new DeleteTransportationType.Command(transportationType.Id);
        await testingServiceScope.SendAsync(command);
        var transportationTypeResponse = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes.CountAsync(t => t.Id == transportationType.Id));

        // Assert
        transportationTypeResponse.Should().Be(0);
    }

    [Fact]
    public async Task delete_transportationtype_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteTransportationType.Command(badId);
        Func<Task> act = () => testingServiceScope.SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task can_softdelete_transportationtype_from_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeTransportationTypeOne = new FakeTransportationTypeBuilder().Build();
        await testingServiceScope.InsertAsync(fakeTransportationTypeOne);
        var transportationType = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes
            .FirstOrDefaultAsync(t => t.Id == fakeTransportationTypeOne.Id));

        // Act
        var command = new DeleteTransportationType.Command(transportationType.Id);
        await testingServiceScope.SendAsync(command);
        var deletedTransportationType = await testingServiceScope.ExecuteDbContextAsync(db => db.TransportationTypes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == transportationType.Id));

        // Assert
        deletedTransportationType?.IsDeleted.Should().BeTrue();
    }
}