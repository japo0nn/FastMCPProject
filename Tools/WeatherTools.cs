using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastMCPProject.Interfaces;
using ModelContextProtocol.Server;

namespace FastMCPProject.Tools;

public class WeatherTools(IOpenWeahterService weahterService)
{
    private readonly IOpenWeahterService _weahterService = weahterService;

    [McpServerTool]
    [Description("Gets current weather conditions for the specified city.")]
    public async Task<string> GetCurrentWeather(
        [Description("The city name to get weather for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null,
        [Description("Optional: Units for temperature ('metric', 'imperial' or 'standard')")] string? units = "metric")
    {
        if (string.IsNullOrWhiteSpace(city))
            return "City name must not be empty.";
        if (units != "metric" && units != "imperial" && units != "standard")
            return "Invalid units specified. Use 'metric', 'imperial', or 'standard'.";

        try
        {
            var weather = await _weahterService.GetWeatherAsync(city, countryCode, units);
            return $"Current weather in {city}:\n" +
                   $"Condition: {weather.Weather.FirstOrDefault()?.Description ?? "N/A"}\n" +
                   $"Temperature:\n\tMin: {weather.Main.TempMin}\n\tMax: {weather.Main.TempMax}\n\tFeels like: {weather.Main.FeelsLike}\n" +
                   $"Humidity: {weather.Main.Humidity}%\n" +
                   $"Pressure: {weather.Main.Pressure} hPa\n";
        }
        catch (Exception ex)
        {
            return $"Error retrieving weather data: {ex.Message}";
        }
    }

    [McpServerTool]
    [Description("Gets weather forecast for the specified city.")]
    public async Task<string> GetForecast(
        [Description("The city name to get forecast for")] string city,
        [Description("Optional: Country code (e.g., 'US', 'UK')")] string? countryCode = null,
        [Description("Optional: Units for temperature ('metric', 'imperial' or 'standard')")] string units = "metric",
        [Description("Optional: Number of days to forecast (default is 1, max is 5)")] int days = 1)
    {
        if (string.IsNullOrWhiteSpace(city))
            return "City name must not be empty.";
        if (days < 1 || days > 5)
            return "Days must be between 1 and 5.";
        if (units != "metric" && units != "imperial" && units != "standard")
            return "Invalid units specified. Use 'metric', 'imperial', or 'standard'.";

        try
        {
            var forecast = await _weahterService.GetForecastAsync(city, countryCode, units, days * 8);
            var result = new StringBuilder($"Daily forecast for {city}:\n");
            foreach (var item in forecast.Forecasts)
            {
                result.AppendLine($"Date: {item.DateTime}\n" +
                                  $"Condition: {item.Weather.FirstOrDefault()?.Description ?? "N/A"}\n" +
                                  $"Temperature:\n\tMin: {item.Main.TempMin}\n\tMax: {item.Main.TempMax}\n\tFeels like: {item.Main.FeelsLike}\n" +
                                  $"Humidity: {item.Main.Humidity}%\n" +
                                  $"Pressure: {item.Main.Pressure} hPa\n");
            }
            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"Error retrieving daily forecast: {ex.Message}";
        }
    }
}
