using Newtonsoft.Json;

namespace FastMCPProject.Models;

/// <summary>
/// Represents a daily forecast item with date, main weather info, and weather conditions.
/// </summary>
public class DailyForecastItem
{
    /// <summary>
    /// Date and time of the forecasted data (in string format).
    /// </summary>
    [JsonProperty("dt_txt")]
    public string DateTime { get; set; } = string.Empty;

    /// <summary>
    /// Main weather information for the forecasted time.
    /// </summary>
    [JsonProperty("main")]
    public MainInfo Main { get; set; } = new();

    /// <summary>
    /// Array of weather conditions for the forecasted time.
    /// </summary>
    [JsonProperty("weather")]
    public WeatherInfo[] Weather { get; set; } = [];
}