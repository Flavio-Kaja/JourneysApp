namespace JourneyService.Domain.TransportationTypes.Services;

using JourneyService.Domain.TransportationTypes;
using JourneyService.Databases;
using JourneyService.Services;

public interface ITransportationTypeRepository : IGenericRepository<TransportationType>
{
}

public sealed class TransportationTypeRepository : GenericRepository<TransportationType>, ITransportationTypeRepository
{

    public TransportationTypeRepository(JourneysDbContext dbContext) : base(dbContext)
    {
    }
}
