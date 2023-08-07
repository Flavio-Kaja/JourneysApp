namespace JourneyService.SharedTestHelpers.Fakes.TransportationType;

using AutoBogus;
using JourneyService.Domain.TransportationTypes;
using JourneyService.Domain.TransportationTypes.Dtos;

public sealed class FakeTransportationTypeForCreationDto : AutoFaker<PostTransportationTypeDto>
{
    public FakeTransportationTypeForCreationDto()
    {
    }
}