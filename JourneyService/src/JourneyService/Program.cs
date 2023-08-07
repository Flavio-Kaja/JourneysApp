using Serilog;
using Hellang.Middleware.ProblemDetails;
using JourneyService.Extensions.Application;
using JourneyService.Extensions.Host;
using JourneyService.Extensions.Services;
using JourneyService.Databases;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLoggingConfiguration(builder.Environment);

builder.ConfigureServices();
var app = builder.Build();

using var scope = app.Services.CreateScope();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseProblemDetails();

app.UseHttpsRedirection();

app.UseCors("JourneyServiceCorsPolicy");

app.UseSerilogRequestLogging();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/api/health");
    endpoints.MapControllers();
});

app.UseSwaggerExtension(builder.Configuration, builder.Environment);

try
{
    Log.Information("Starting application");
    await app.RunAsync();
}
catch (Exception e)
{
    Log.Error(e, "The application failed to start correctly");
    throw;
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
}

public partial class Program { }