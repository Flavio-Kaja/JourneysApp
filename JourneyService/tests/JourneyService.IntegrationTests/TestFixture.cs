namespace JourneyService.IntegrationTests;

using JourneyService.Extensions.Services;
using JourneyService.Databases;
using JourneyService.Resources;
using JourneyService.SharedTestHelpers.Utilities;
using Configurations;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

[CollectionDefinition(nameof(TestFixture))]
public class TestFixtureCollection : ICollectionFixture<TestFixture> { }

public class TestFixture : IAsyncLifetime
{
    public static IServiceScopeFactory BaseScopeFactory;
    private readonly TestcontainerDatabase _dbContainer = DbSetup();

    public async Task InitializeAsync()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = Consts.Testing.IntegrationTestingEnvName
        });

        await _dbContainer.StartAsync();
        builder.Configuration.GetSection(ConnectionStringOptions.SectionName)[ConnectionStringOptions.JourneyServiceKey] = _dbContainer.ConnectionString;
        await RunMigration(_dbContainer.ConnectionString);


        builder.ConfigureServices();
        var services = builder.Services;

        // add any mock services here
        services.ReplaceServiceWithSingletonMock<IHttpContextAccessor>();

        var provider = services.BuildServiceProvider();
        BaseScopeFactory = provider.GetService<IServiceScopeFactory>();
        SetupDateAssertions();
    }

    private static async Task RunMigration(string connectionString)
    {
        var options = new DbContextOptionsBuilder<JourneysDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        var context = new JourneysDbContext(options, null, null, null);
        await context?.Database?.MigrateAsync();
    }

    private static TestcontainerDatabase DbSetup()
    {
        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "db",
                Username = "postgres",
                Password = "postgres"
            })
            .WithName($"IntegrationTesting_JourneyService_{Guid.NewGuid()}")
            .WithImage("postgres:latest")
            .Build();
    }

    private class RmqConfig
    {
        public IContainer Container { get; set; }
        public int Port { get; set; }
    }

    private static RmqConfig RmqSetup()
    {
        var freePort = DockerUtilities.GetFreePort();
        return new RmqConfig
        {
            Container = new ContainerBuilder()
                .WithImage("masstransit/rabbitmq")
                .WithPortBinding(freePort, 5672)
                .WithName($"IntegrationTesting_RMQ_{Guid.NewGuid()}")
                .Build(),
            Port = freePort
        };
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private static void SetupDateAssertions()
    {
        // close to equivalency required to reconcile precision differences between EF and Postgres
        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(ctx => ctx.Subject
                .Should()
                .BeCloseTo(ctx.Expectation, 1.Seconds())).WhenTypeIs<DateTime>();
            options.Using<DateTimeOffset>(ctx => ctx.Subject
                .Should()
                .BeCloseTo(ctx.Expectation, 1.Seconds())).WhenTypeIs<DateTimeOffset>();

            return options;
        });
    }
}

public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection ReplaceServiceWithSingletonMock<TService>(this IServiceCollection services)
        where TService : class
    {
        services.RemoveAll(typeof(TService));
        services.AddSingleton(_ => Mock.Of<TService>());
        return services;
    }
}
