using Newtonsoft.Json;

namespace FastMCPProject.Models;

/// <summary>
/// Represents the weather forecast response containing a list of daily forecasts.
/// </summary>
public class ForecastResponse
{
    /// <summary>
    /// Collection of daily forecast items.
    /// </summary>
    [JsonProperty("list")]
    public ICollection<DailyForecastItem> Forecasts { get; set; } = [];
}