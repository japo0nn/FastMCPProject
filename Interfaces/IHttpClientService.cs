namespace FastMCPProject.Interfaces;

public interface IHttpClientService
{
    Task<T?> SendAsync<T>(string uri, HttpMethod httpMethod, object? body = null, string? apiKey = null, CancellationToken cancellationToken = default);
}