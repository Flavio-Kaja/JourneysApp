namespace JourneyService.Domain.TransportationTypes.Features;

using JourneyService.Domain.TransportationTypes;
using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.Domain.TransportationTypes.Services;
using JourneyService.Exceptions;
using JourneyService.Services;
using Mappings;
using MediatR;

public static class UpdateTransportationType
{
    public sealed class Command : IRequest
    {
        public readonly Guid Id;
        public readonly PostTransportationTypeDto UpdatedTransportationTypeData;

        public Command(Guid id, PostTransportationTypeDto updatedTransportationTypeData)
        {
            Id = id;
            UpdatedTransportationTypeData = updatedTransportationTypeData;
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly ITransportationTypeRepository _transportationTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Handler> _logger;
        public Handler(ITransportationTypeRepository transportationTypeRepository, IUnitOfWork unitOfWork, ILogger<Handler> logger)
        {
            _transportationTypeRepository = transportationTypeRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var transportationTypeToUpdate = await _transportationTypeRepository.GetById(request.Id, cancellationToken: cancellationToken);
            _logger.LogInformation("Request to update transportation type {@transportationTypeToUpdate} with data {@data}", transportationTypeToUpdate, request);
            if (transportationTypeToUpdate is null)
            {
                _logger.LogError("Transportation type {@request} not found", request);
                throw new NotFoundException($"Transportation type with id: {request.Id}");
            }
            transportationTypeToUpdate.Update(request.UpdatedTransportationTypeData);

            _transportationTypeRepository.Update(transportationTypeToUpdate);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}