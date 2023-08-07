namespace JourneyService.Domain.TransportationTypes.Features;

using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.Domain.TransportationTypes.Services;
using JourneyService.Wrappers;
using Mappings;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetTransportationTypeList
{
    public sealed class Query : IRequest<IEnumerable<TransportationTypeDto>>
    {
        public Query()
        {
        }
    }

    public sealed class Handler : IRequestHandler<Query, IEnumerable<TransportationTypeDto>>
    {
        private readonly ITransportationTypeRepository _transportationTypeRepository;
        private readonly ILogger<Handler> _logger;
        public Handler(ITransportationTypeRepository transportationTypeRepository, ILogger<Handler> logger)
        {
            _transportationTypeRepository = transportationTypeRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TransportationTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving transportation data");
            var collection = _transportationTypeRepository.Query().AsNoTracking();
            return await collection.ToTransportationTypeDtoQueryable().ToListAsync(cancellationToken: cancellationToken);
        }
    }
}