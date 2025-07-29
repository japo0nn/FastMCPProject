using FastMCPProject.Models;

namespace FastMCPProject.Interfaces;

/// <summary>
/// Provides methods to interact with the OpenWeather API.
/// </summary>
public interface IOpenWeahterService
{
    /// <summary>
    /// Gets the current weather for a specified city.
    /// </summary>
    /// <param name="city">The city name.</param>
    /// <param name="countryCode">Optional country code.</param>
    /// <param name="units">Optional units (metric, imperial, etc.).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The current weather response.</returns>
    Task<CurrentWeatherResponse> GetWeatherAsync(string city, string? countryCode = null, string? units = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the weather forecast for a specified city.
    /// </summary>
    /// <param name="city">The city name.</param>
    /// <param name="countryCode">Optional country code.</param>
    /// <param name="units">Optional units (metric, imperial, etc.).</param>
    /// <param name="count">Number of forecast days.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The forecast response.</returns>
    Task<ForecastResponse> GetForecastAsync(string city, string? countryCode = null, string? units = null, int count = 1, CancellationToken cancellationToken = default);
}