namespace JourneyService.Domain.TransportationTypes.Features;

using JourneyService.Domain.TransportationTypes.Services;
using JourneyService.Services;
using MediatR;

public static class DeleteTransportationType
{
    public sealed class Command : IRequest
    {
        public readonly Guid Id;

        public Command(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly ITransportationTypeRepository _transportationTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(ITransportationTypeRepository transportationTypeRepository, IUnitOfWork unitOfWork)
        {
            _transportationTypeRepository = transportationTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recordToDelete = await _transportationTypeRepository.GetById(request.Id, cancellationToken: cancellationToken);
            _transportationTypeRepository.Remove(recordToDelete);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}