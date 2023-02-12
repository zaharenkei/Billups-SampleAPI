using System.Text.Json;
using Microsoft.Extensions.Options;
using SampleAPI.Infrastructure.Exceptions;
using SampleAPI.Services.ExternalClients.Models;

namespace SampleAPI.Services.ExternalClients;

/// <inheritdocs/>
public class RandomClient : IRandomClient
{
    private readonly HttpClient httpClient;
    private readonly RandomClientSettings clientSettings;
    private readonly ILogger<RandomClient> logger;

    public RandomClient(
        IHttpClientFactory httpClientFactory,
        IOptions<RandomClientSettings> clientSettings,
        ILogger<RandomClient> logger)
    {
        httpClient = httpClientFactory.CreateClient(GetType().Name);
        this.clientSettings = clientSettings.Value;
        this.logger = logger;
    }

    /// <inheritdocs/>
    public async Task<RandomResponse> GetRandomNumber(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (clientSettings == null || string.IsNullOrEmpty(clientSettings.Uri))
        {
            throw new ExternalClientException($"Service {GetType().Name} wasn't configured, so proper functioning couldn't be achieved.");
        }

        logger.LogDebug($"Calling {GetType().Name} service at {clientSettings.Uri}.");

        using var response = await httpClient.GetAsync(clientSettings.Uri, token);
        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalClientException($"Service {GetType().Name} failed to reach external resource." +
                                              $"StatusCode: {response.StatusCode}; Uri: {clientSettings.Uri}.");
        }

        var entity = await response.Content.ReadFromJsonAsync<RandomResponse>(new JsonSerializerOptions(), token);
        if (entity == null)
        {
            throw new ExternalClientException($"Service {GetType().Name} failed to read response of external resource." +
                                              $"StatusCode: {response.StatusCode}; Uri: {clientSettings.Uri}.");
        }

        return entity;
    }
}