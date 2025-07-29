using Newtonsoft.Json;

namespace FastMCPProject.Models;

/// <summary>
/// Represents the geographical coordinates for a location.
/// </summary>
public class GeocodeResponse
{
    /// <summary>
    /// Latitude of the location.
    /// </summary>
    [JsonProperty("lat")]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude of the location.
    /// </summary>
    [JsonProperty("lon")]
    public double Longitude { get; set; }
}