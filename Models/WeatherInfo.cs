using Newtonsoft.Json;

namespace FastMCPProject.Models;

/// <summary>
/// Represents weather condition information returned by the API.
/// </summary>
public class WeatherInfo
{
    /// <summary>
    /// Weather condition ID.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Group of weather parameters (Rain, Snow, etc.).
    /// </summary>
    [JsonProperty("main")]
    public string Main { get; set; } = string.Empty;

    /// <summary>
    /// Weather condition within the group.
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}