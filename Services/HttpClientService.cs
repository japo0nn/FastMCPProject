using System.Text;
using FastMCPProject.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace FastMCPProject.Services;

public class HttpClientService(HttpClient httpClient, ILogger<HttpClientService> logger) : IHttpClientService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<HttpClientService> _logger = logger;

    public async Task<T?> SendAsync<T>(string uri, HttpMethod httpMethod, object? body = null, string? token = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending HTTP {Method} request to {Uri}", httpMethod, uri);
            using var request = new HttpRequestMessage(httpMethod, uri);

            if (body is not null && httpMethod != HttpMethod.Get && httpMethod != HttpMethod.Delete)
            {
                string json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                _logger.LogDebug("Request body: {Body}", json);
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }

            var response = await _httpClient.SendAsync(request, cancellationToken);
            _logger.LogInformation("Received HTTP response: {StatusCode} {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogDebug("Response body: {Body}", responseContent);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            _logger.LogError("Request failed with status code {StatusCode}: {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending HTTP request to {Uri}", uri);
            throw new InvalidOperationException($"Error sending HTTP request: {ex.Message}", ex);
        }
    }
}