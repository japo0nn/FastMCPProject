using Newtonsoft.Json;

namespace FastMCPProject.Models;

/// <summary>
/// Represents the response from the current weather API.
/// </summary>
public class CurrentWeatherResponse
{
    /// <summary>
    /// List of weather conditions.
    /// </summary>
    [JsonProperty("weather")]
    public ICollection<WeatherInfo> Weather { get; set; } = [];

    /// <summary>
    /// Main weather information (temperature, pressure, etc.).
    /// </summary>
    [JsonProperty("main")]
    public MainInfo Main { get; set; } = new();

    /// <summary>
    /// Geographical coordinates of the location.
    /// </summary>
    [JsonProperty("coord")]
    public GeocodeResponse Coordinate { get; set; } = new();
}