namespace JourneyService.SharedTestHelpers.Fakes.TransportationType;

using JourneyService.Domain.TransportationTypes;
using JourneyService.Domain.TransportationTypes.Dtos;

public class FakeTransportationTypeBuilder
{
    private PostTransportationTypeDto _creationData = new FakeTransportationTypeForCreation().Generate();

    public FakeTransportationTypeBuilder WithModel(PostTransportationTypeDto model)
    {
        _creationData = model;
        return this;
    }

    public FakeTransportationTypeBuilder WithType(string type)
    {
        _creationData.Type = type;
        return this;
    }

    public TransportationType Build()
    {
        var result = TransportationType.Create(_creationData);
        return result;
    }
}