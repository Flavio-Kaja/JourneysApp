namespace JourneyService.Domain.TransportationTypes.Features;

using JourneyService.Domain.TransportationTypes.Services;
using JourneyService.Domain.TransportationTypes;
using JourneyService.Domain.TransportationTypes.Dtos;
using JourneyService.Services;
using Mappings;
using MediatR;

public static class AddTransportationType
{
    public sealed class Command : IRequest<TransportationTypeDto>
    {
        public readonly PostTransportationTypeDto TransportationTypeToAdd;

        public Command(PostTransportationTypeDto transportationTypeToAdd)
        {
            TransportationTypeToAdd = transportationTypeToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, TransportationTypeDto>
    {
        private readonly ITransportationTypeRepository _transportationTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(ITransportationTypeRepository transportationTypeRepository, IUnitOfWork unitOfWork)
        {
            _transportationTypeRepository = transportationTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransportationTypeDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var transportationType = TransportationType.Create(request.TransportationTypeToAdd);

            await _transportationTypeRepository.Add(transportationType, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return transportationType.ToTransportationTypeDto();
        }
    }
}