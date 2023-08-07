namespace JourneyService.Domain.Journeys.Services;

using JourneyService.Domain.Journeys;
using JourneyService.Databases;
using JourneyService.Services;

public interface IJourneyRepository : IGenericRepository<Journey>
{
}

public sealed class JourneyRepository : GenericRepository<Journey>, IJourneyRepository
{

    public JourneyRepository(JourneysDbContext dbContext) : base(dbContext)
    {
    }
}
