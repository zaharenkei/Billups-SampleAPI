using SampleAPI.Infrastructure.Exceptions;
using SampleAPI.Services.ExternalClients.Models;

namespace SampleAPI.Services.ExternalClients;

/// <summary>
/// Service for retrieving random numbers.
/// </summary>
public interface IRandomClient
{
    /// <summary>
    /// Calls external service to obtain random number.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>External service response.</returns>
    /// <exception cref="ExternalClientException">Throws on service issues, either configuration or accessibility.</exception>
    /// <remarks>
    ///     Polly could be used there to reduce the number of faults.
    ///     Still, as service looks pretty stable, i'm not sure that it's in scope.
    /// </remarks>
    public Task<RandomResponse> GetRandomNumber(CancellationToken token);
}