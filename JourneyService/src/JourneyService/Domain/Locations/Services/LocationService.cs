using JourneyService.Domain.Locations;
using JourneyService.Exceptions;
using JourneyService.Infrastructure.Models;
using JourneyService.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JourneyService.Domain.Locations.Services;


/// <summary>
/// Interface that defines the retrieval of geo location data from the open street map public api
/// </summary>
public interface ILocationService : IJourneyServiceScopedService
{
    Task<GeoLocation> GetLocationByStringAsync(string search);
    Task<Location> GetOrCreateLocationAsync(string locationName, CancellationToken cancellationToken);
}


/// <summary>
/// Service that handles the retrieval of geo location data from the open street map public api
/// </summary>
public class LocationService : ILocationService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LocationService> _logger;

    public LocationService(IHttpClientFactory clientFactory, ILocationRepository locationRepository, IUnitOfWork unitOfWork, ILogger<LocationService> logger)
    {
        _clientFactory = clientFactory;
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GeoLocation> GetLocationByStringAsync(string search)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"search?format=json&q={search}");
            var client = _clientFactory.CreateClient("OpenStreetMap");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537");
            _logger.LogInformation("Calling the open street api with request {@request}", request);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var locations = await JsonSerializer.DeserializeAsync<List<GeoLocation>>(responseStream, options);
                return locations.FirstOrDefault();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Response unseccessful when calling the open street api {@errorContent}", errorContent);
                throw new Exception($"Failed to get the location. Status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("Response unseccessful when calling the open street api,see exception details {@e}", e);
            throw new Exception("An error occurred while sending the request.", e);
        }
        catch (JsonException e)
        {
            _logger.LogError("Parsing response failed when calling the open street api,see exception details {@e}", e);
            throw new Exception("An error occurred while deserializing the response.", e);
        }
    }

    /// <summary>
    /// Gets or adds a new location
    /// </summary>
    /// <param name="locationName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="LocationNotFoundException"></exception>
    public async Task<Location> GetOrCreateLocationAsync(string locationName, CancellationToken cancellationToken)
    {
        var locationData = await GetLocationByStringAsync(locationName);
        if (locationData == null)
        {
            throw new LocationNotFoundException(locationName);
        }

        var existingLocation = await _locationRepository.Query().AsNoTracking()
            .FirstOrDefaultAsync(l => l.Latitude == locationData.Latitude && l.Longitude == locationData.Longitude, cancellationToken: cancellationToken);

        if (existingLocation == null)
        {
            existingLocation = Location.Create(locationData.Latitude, locationData.Longitude, locationName);
            await _locationRepository.Add(existingLocation, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);
        }

        return existingLocation;
    }
}