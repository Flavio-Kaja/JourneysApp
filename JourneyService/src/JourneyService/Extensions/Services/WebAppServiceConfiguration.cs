namespace JourneyService.Extensions.Services;

using JourneyService.Middleware;
using JourneyService.Services;
using Configurations;
using System.Text.Json.Serialization;
using Serilog;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Resources;
using Sieve.Services;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using FluentValidation;
using JourneyService.Domain.Journeys.Dtos;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddProblemDetails(ProblemDetailsConfigurationExtension.ConfigureProblemDetails)
            .AddProblemDetailsConventions();

        builder.Services.AddCorsService("JourneyServiceCorsPolicy", builder.Environment);
        builder.OpenTelemetryRegistration(builder.Configuration, "JourneyService");
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
        builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);
        builder.Services.AddMemoryCache();
        builder.Services.AddAppAuthentication(builder.Environment, builder.Configuration);
        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddApiVersioningExtension();

        builder.Services.AddScoped<SieveProcessor>();

        // register boundary services
        builder.Services.AddBoundaryServices(Assembly.GetExecutingAssembly());

        builder.Services.AddMvc();

        builder.Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<PostJourneyDtoValidator>();

        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerExtension(builder.Configuration);
    }

    /// <summary>
    /// Registers all services in the assembly of the given interface.
    /// </summary>
    private static void AddBoundaryServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

        foreach (var assembly in assemblies)
        {
            var rules = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IJourneyServiceScopedService)) == typeof(IJourneyServiceScopedService));

            foreach (var rule in rules)
            {
                foreach (var @interface in rule.GetInterfaces())
                {
                    services.Add(new ServiceDescriptor(@interface, rule, ServiceLifetime.Scoped));
                }
            }
        }
    }
}