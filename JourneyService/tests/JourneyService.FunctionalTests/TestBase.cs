namespace JourneyService.FunctionalTests;

using JourneyService.Databases;
using JourneyService;
using AutoBogus;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
 
[Collection(nameof(TestBase))]
public class TestBase : IDisposable
{
    private static IServiceScopeFactory _scopeFactory;
    protected static HttpClient FactoryClient  { get; private set; }

    public TestBase()
    {
        var factory = new TestingWebApplicationFactory();
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        FactoryClient = factory.CreateClient(new WebApplicationFactoryClientOptions());

        AutoFaker.Configure(builder =>
        {
            // configure global autobogus settings here
            builder.WithDateTimeKind(DateTimeKind.Utc)
                .WithRecursiveDepth(3)
                .WithTreeDepth(1)
                .WithRepeatCount(1);
        });
    }
    
    public void Dispose()
    {
        FactoryClient.Dispose();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<ISender>();
        return await mediator.Send(request);
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<JourneysDbContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<JourneysDbContext>();
        context.Add(entity);
        await context.SaveChangesAsync();
    }

    public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JourneysDbContext>();
        await action(scope.ServiceProvider);
    }

    public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JourneysDbContext>();
        return await action(scope.ServiceProvider);
    }

    public static Task ExecuteDbContextAsync(Func<JourneysDbContext, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<JourneysDbContext>()));

    public static Task ExecuteDbContextAsync(Func<JourneysDbContext, ValueTask> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<JourneysDbContext>()).AsTask());

    public static Task ExecuteDbContextAsync(Func<JourneysDbContext, IMediator, Task> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<JourneysDbContext>(), sp.GetService<IMediator>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<JourneysDbContext, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<JourneysDbContext>()));

    public static Task<T> ExecuteDbContextAsync<T>(Func<JourneysDbContext, ValueTask<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<JourneysDbContext>()).AsTask());

    public static Task<T> ExecuteDbContextAsync<T>(Func<JourneysDbContext, IMediator, Task<T>> action)
        => ExecuteScopeAsync(sp => action(sp.GetService<JourneysDbContext>(), sp.GetService<IMediator>()));

    public static Task<int> InsertAsync<T>(params T[] entities) where T : class
    {
        return ExecuteDbContextAsync(db =>
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Add(entity);
            }
            return db.SaveChangesAsync();
        });
    }
}