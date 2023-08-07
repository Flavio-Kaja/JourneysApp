using System.Text.Json.Serialization;

namespace JourneyService.Infrastructure.Models
{
    public class GeoLocation
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("lon")]
        public string Lon { get; set; }

        [JsonIgnore]
        public double Latitude
        {
            get
            {
                if (double.TryParse(Lat, out var latitude))
                {
                    return latitude;
                }
                return 0;
            }
        }

        [JsonIgnore]
        public double Longitude
        {
            get
            {
                if (double.TryParse(Lon, out var longitude))
                {
                    return longitude;
                }
                return 0;
            }
        }
    }
}
