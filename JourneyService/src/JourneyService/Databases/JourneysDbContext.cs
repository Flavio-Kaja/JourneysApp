namespace JourneyService.Databases;

using JourneyService.Domain;
using JourneyService.Databases.EntityConfigurations;
using JourneyService.Services;
using Configurations;
using MediatR;
using JourneyService.Domain.Journeys;
using JourneyService.Domain.Locations;
using JourneyService.Domain.TransportationTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

public sealed class JourneysDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JourneysDbContext(
        DbContextOptions<JourneysDbContext> options, ICurrentUserService currentUserService, IMediator mediator, IDateTimeProvider dateTimeProvider) : base(options)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
    }

    #region DbSet Region
    public DbSet<Journey> Journeys { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<TransportationType> TransportationTypes { get; set; }
    #endregion DbSet Region

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.FilterSoftDeletedRecords();

        #region Entity Database Config Region
        modelBuilder.ApplyConfiguration(new JourneyConfiguration());
        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new TransportationTypeConfiguration());
        #endregion Entity Database Config Region
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        var result = base.SaveChanges();
        DispatchDomainEvents().GetAwaiter().GetResult();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEvents();
        return result;
    }

    private async Task DispatchDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries<BaseEntity>()
            .Select(po => po.Entity)
            .Where(po => po.DomainEvents.Any())
            .ToArray();

        foreach (var entity in domainEventEntities)
        {
            var events = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var entityDomainEvent in events)
                await _mediator.Publish(entityDomainEvent);
        }
    }

    private void UpdateAuditFields()
    {
        var now = _dateTimeProvider.DateTimeUtcNow;
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.UpdateCreationProperties(now, _currentUserService?.UserId);
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService?.UserId);
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService?.UserId);
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.UpdateModifiedProperties(now, _currentUserService?.UserId);
                    entry.Entity.UpdateIsDeleted(true);
                    break;
            }
        }
    }
}

public static class Extensions
{
    public static void FilterSoftDeletedRecords(this ModelBuilder modelBuilder)
    {
        Expression<Func<BaseEntity, bool>> filterExpr = e => !e.IsDeleted;
        foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes()
            .Where(m => m.ClrType.IsAssignableTo(typeof(BaseEntity))))
        {
            // modify expression to handle correct child type
            var parameter = Expression.Parameter(mutableEntityType.ClrType);
            var body = ReplacingExpressionVisitor
                .Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
            var lambdaExpression = Expression.Lambda(body, parameter);

            // set filter
            mutableEntityType.SetQueryFilter(lambdaExpression);
        }
    }
}
