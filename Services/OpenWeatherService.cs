using FastMCPProject.Interfaces;
using FastMCPProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FastMCPProject.Services;

/// <summary>
/// Service for interacting with the OpenWeather API to retrieve weather and forecast data.
/// </summary>
public class OpenWeatherService(IHttpClientService httpClientService, IConfiguration configuration, ILogger<OpenWeatherService> logger) : IOpenWeahterService
{
    private readonly IHttpClientService _httpClientService = httpClientService;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<OpenWeatherService> _logger = logger;

    /// <inheritdoc />
    public async Task<CurrentWeatherResponse> GetWeatherAsync(string city, string? countryCode = null, string? units = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting weather for city: {City}, country: {CountryCode}, units: {Units}", city, countryCode, units);
            
            var geocode = await GetGeocode(city, countryCode, cancellationToken);

            var baseUri = _configuration["OpenWeather:BaseUri"];
            var weatherEndpoint = _configuration["OpenWeather:Endpoints:Weather"];
            var apiKey = _configuration["OpenWeather:ApiKey"];
            
            _logger.LogInformation("Configuration values - BaseUri: {BaseUri}, WeatherEndpoint: {WeatherEndpoint}, ApiKey: {ApiKey}", 
                baseUri, weatherEndpoint, apiKey);
            
            var uri = $"{baseUri}{weatherEndpoint}?appid={apiKey}";
            uri += $"&lat={geocode.Latitude}&lon={geocode.Longitude}";
            if (!string.IsNullOrEmpty(units))
            {
                uri += $"&units={Uri.EscapeDataString(units)}";
            }

            _logger.LogInformation("Requesting weather from: {Uri}", uri);
            var weather = await _httpClientService.SendAsync<CurrentWeatherResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);

            if (weather == null)
            {
                _logger.LogError("Failed to retrieve weather data for city '{City}'.", city);
                throw new InvalidOperationException($"Failed to retrieve weather data for city '{city}'.");
            }

            _logger.LogInformation("Weather data retrieved successfully for city: {City}", city);
            return weather;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while getting weather for city: {City}", city);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<ForecastResponse> GetForecastAsync(string city, string? countryCode = null, string? units = null, int count = 1, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting forecast for city: {City}, country: {CountryCode}, units: {Units}, count: {Count}", city, countryCode, units, count);
            var geocode = await GetGeocode(city, countryCode, cancellationToken);

            var baseUri = _configuration["OpenWeather:BaseUri"];
            var forecastEndpoint = _configuration["OpenWeather:Endpoints:Forecast"];
            var apiKey = _configuration["OpenWeather:ApiKey"];

            _logger.LogInformation("Configuration values - BaseUri: {BaseUri}, WeatherEndpoint: {WeatherEndpoint}, ApiKey: {ApiKey}", 
                baseUri, forecastEndpoint, apiKey);

            var uri = $"{baseUri}{forecastEndpoint}?appid={apiKey}";
            uri += $"&lat={geocode.Latitude}&lon={geocode.Longitude}";
            uri += $"&cnt={count}";
            if (!string.IsNullOrEmpty(units))
            {
                uri += $"&units={Uri.EscapeDataString(units)}";
            }

            _logger.LogInformation("Requesting daily forecast from: {Uri}", uri);

            var forecast = await _httpClientService.SendAsync<ForecastResponse>(uri, HttpMethod.Get, cancellationToken: cancellationToken);

            if (forecast == null)
            {
                _logger.LogError("Failed to retrieve daily forecast for city '{City}'.", city);
                throw new InvalidOperationException($"Failed to retrieve daily forecast for city '{city}'.");
            }

            _logger.LogInformation("Forecast data retrieved successfully for city: {City}", city);
            return forecast;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while getting forecast for city: {City}", city);
            throw;
        }
    }

    /// <summary>
    /// Gets the geographical coordinates for a given city and optional country code.
    /// </summary>
    /// <param name="city">The city name.</param>
    /// <param name="countryCode">Optional country code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The geocode response for the city.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no geocode is found for the city.</exception>
    private async Task<GeocodeResponse> GetGeocode(string city, string? countryCode = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting geocode for city: {City}, country: {CountryCode}", city, countryCode);

            var baseUri = _configuration["OpenWeather:BaseUri"];
            var geocodeEndpoint = _configuration["OpenWeather:Endpoints:Geocode"];
            var apiKey = _configuration["OpenWeather:ApiKey"];

            _logger.LogInformation("Configuration values - BaseUri: {BaseUri}, GeocodeEndpoint: {GeocodeEndpoint}, ApiKey: {ApiKey}", 
                baseUri, geocodeEndpoint, apiKey);

            var uri = $"{baseUri}{geocodeEndpoint}?appid={apiKey}";
            uri += $"&limit=1&q={Uri.EscapeDataString(city)}";
            if (!string.IsNullOrEmpty(countryCode))
            {
                uri += $",{Uri.EscapeDataString(countryCode)}";
            }

            _logger.LogInformation("Requesting geocode from: {Uri}", uri);

            var geocode = await _httpClientService.SendAsync<GeocodeResponse[]>(uri, HttpMethod.Get, cancellationToken: cancellationToken);

            if (geocode is null || !geocode.Any())
            {
                _logger.LogError("No geocode found for city '{City}'.", city);
                throw new InvalidOperationException($"No geocode found for city '{city}'.");
            }

            _logger.LogInformation("Geocode retrieved successfully for city: {City}", city);
            return geocode.First();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while getting geocode for city: {City}", city);
            throw;
        }
    }
}