namespace SampleAPI.Handlers;

/// <summary>
/// Handler to execute either Command or Query, where no RequestParameters were specified.
/// </summary>
/// <typeparam name="TResponse">Entity that handler will response with.</typeparam>
public interface IHandler<TResponse>
{
    /// <summary>
    /// Executes handler.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Result of execution.</returns>
    ValueTask<TResponse> Execute(CancellationToken token);
}

/// <summary>
/// Handler to execute either Command or Query, where no RequestParameters were specified.
/// </summary>
/// <typeparam name="TRequest">Entity that handler will process.</typeparam>
/// <typeparam name="TResponse">Entity that handler will response with.</typeparam>
public interface IHandler<in TRequest, TResponse>
{
    /// <summary>
    /// Executes handler.
    /// </summary>
    /// <param name="request">Request that will be processed.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Result of execution.</returns>
    ValueTask<TResponse> Execute(TRequest request, CancellationToken token);
}