namespace JourneyService.Services;

using JourneyService.Databases;

public interface IUnitOfWork : IJourneyServiceScopedService
{
    Task<int> CommitChanges(CancellationToken cancellationToken = default);
}

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly JourneysDbContext _dbContext;

    public UnitOfWork(JourneysDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
