using Newtonsoft.Json;

namespace FastMCPProject.Models;

/// <summary>
/// Contains main weather parameters such as temperature, pressure, and humidity.
/// </summary>
public class MainInfo
{
    /// <summary>
    /// Current temperature.
    /// </summary>
    [JsonProperty("temp")]
    public double Temp { get; set; }

    /// <summary>
    /// Perceived temperature.
    /// </summary>
    [JsonProperty("feels_like")]
    public double FeelsLike { get; set; }

    /// <summary>
    /// Minimum temperature at the moment.
    /// </summary>
    [JsonProperty("temp_min")]
    public double TempMin { get; set; }

    /// <summary>
    /// Maximum temperature at the moment.
    /// </summary>
    [JsonProperty("temp_max")]
    public double TempMax { get; set; }

    /// <summary>
    /// Atmospheric pressure (hPa).
    /// </summary>
    [JsonProperty("pressure")]
    public int Pressure { get; set; }

    /// <summary>
    /// Humidity percentage.
    /// </summary>
    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    /// <summary>
    /// Atmospheric pressure at sea level (hPa).
    /// </summary>
    [JsonProperty("sea_level")]
    public int? SeaLevel { get; set; }

    /// <summary>
    /// Atmospheric pressure at ground level (hPa).
    /// </summary>
    [JsonProperty("grnd_level")]
    public int? GroundLevel { get; set; }
}