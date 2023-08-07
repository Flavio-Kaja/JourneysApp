namespace JourneyService.FunctionalTests;

using JourneyService.Resources;
using JourneyService.SharedTestHelpers.Utilities;
using Configurations;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Xunit;

[CollectionDefinition(nameof(TestBase))]
public class TestingWebApplicationFactoryCollection : ICollectionFixture<TestingWebApplicationFactory> { }

public class TestingWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _dbContainer = DbSetup();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Consts.Testing.FunctionalTestingEnvName);
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var functionalConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            configurationBuilder.AddConfiguration(functionalConfig);
        });

        builder.ConfigureServices(services =>
        {
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        Environment.SetEnvironmentVariable($"{ConnectionStringOptions.SectionName}__{ConnectionStringOptions.JourneyServiceKey}", _dbContainer.ConnectionString);
        // migrations applied in MigrationHostedService
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    private static TestcontainerDatabase DbSetup()
    {
        return new ContainerBuilder<PostgreSqlTestcontainer>()
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
}