namespace JourneyService.Domain.Locations.Services;

using JourneyService.Domain.Locations;
using JourneyService.Databases;
using JourneyService.Services;

public interface ILocationRepository : IGenericRepository<Location>
{
}

public sealed class LocationRepository : GenericRepository<Location>, ILocationRepository
{

    public LocationRepository(JourneysDbContext dbContext) : base(dbContext)
    {
    }
}
