namespace JourneyService.Configurations
{
    public static class OpenStreetMapConfiguration
    {
        public static string GetOpenStreetMapValue(this IConfiguration configuration)
       => configuration.GetSection("OpenStreetMap").Value;
    }
}
