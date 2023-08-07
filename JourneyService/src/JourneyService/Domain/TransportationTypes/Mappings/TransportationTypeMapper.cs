namespace JourneyService.Domain.TransportationTypes.Mappings;

using JourneyService.Domain.TransportationTypes.Dtos;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class TransportationTypeMapper
{
    public static partial TransportationTypeDto ToTransportationTypeDto(this TransportationType transportationType);
    public static partial IQueryable<TransportationTypeDto> ToTransportationTypeDtoQueryable(this IQueryable<TransportationType> transportationType);
}