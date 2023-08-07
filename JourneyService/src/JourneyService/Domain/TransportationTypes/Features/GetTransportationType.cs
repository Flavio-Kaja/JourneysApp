namespace JourneyService.Domain.TransportationTypes.Features;

using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.Domain.TransportationTypes.Services;
using Mappings;
using MediatR;

public static class GetTransportationType
{
    public sealed class Query : IRequest<TransportationTypeDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, TransportationTypeDto>
    {
        private readonly ITransportationTypeRepository _transportationTypeRepository;

        public Handler(ITransportationTypeRepository transportationTypeRepository)
        {
            _transportationTypeRepository = transportationTypeRepository;
        }

        public async Task<TransportationTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _transportationTypeRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return result.ToTransportationTypeDto();
        }
    }
}