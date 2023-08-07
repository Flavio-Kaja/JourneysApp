namespace JourneyService.Extensions.Services;

using JourneyService.Databases;
using JourneyService.Resources;
using JourneyService.Services;
using Configurations;
using Microsoft.EntityFrameworkCore;
using Polly;
using Microsoft.Extensions.DependencyInjection;
using JourneyService.Domain;
using System.Security.Claims;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        // Configured the db context
        var connectionString = configuration.GetConnectionStringOptions().JourneyService;

        services.AddDbContext<JourneysDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(JourneysDbContext).Assembly.FullName))
                            .UseSnakeCaseNamingConvention());

        services.AddHostedService<MigrationHostedService<JourneysDbContext>>();

        // Configured the open street map service to get location cordinates
        services.AddHttpClient("OpenStreetMap", c =>
        {
            c.BaseAddress = new Uri(configuration.GetOpenStreetMapValue());
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddTransientHttpErrorPolicy(p =>
            p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)))
        .AddTransientHttpErrorPolicy(p =>
            p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Permissions.CanCreateJourney, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanCreateJourney));
            options.AddPolicy(Permissions.CanReadJourney, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanReadJourney));
            options.AddPolicy(Permissions.CanUpdateJourney, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanUpdateJourney));
            options.AddPolicy(Permissions.CanDeleteJourney, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanDeleteJourney));
            options.AddPolicy(Permissions.CanFilterJourneys, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanFilterJourneys));
            options.AddPolicy(Permissions.CanCreateTransportation, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanCreateTransportation));
            options.AddPolicy(Permissions.CanReadTransportation, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanReadTransportation));
            options.AddPolicy(Permissions.CanUpdateTransportation, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanUpdateTransportation));
            options.AddPolicy(Permissions.CanDeleteTransportation, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanDeleteTransportation));
            options.AddPolicy(Permissions.CanViewMonthlyRouteDistance, policy => policy.RequireClaim(ClaimTypes.AuthorizationDecision, Permissions.CanViewMonthlyRouteDistance));

        });

    }
}
