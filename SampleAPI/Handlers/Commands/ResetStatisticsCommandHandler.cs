using SampleAPI.Services.DataProviders;

namespace SampleAPI.Handlers.Commands
{
    public class ResetStatisticsCommandHandler : IHandler<string, bool>
    {
        private readonly IUserStatisticsProvider statisticsProvider;

        public ResetStatisticsCommandHandler(IUserStatisticsProvider statisticsProvider)
        {
            this.statisticsProvider = statisticsProvider;
        }

        /// <summary>
        /// Resets the statistics based on connection id.
        /// </summary>
        /// <param name="connectionId">ConnectionId to be used.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>True if statistics was cleared successfully.</returns>
        public ValueTask<bool> Execute(string connectionId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return statisticsProvider.Delete(connectionId, token);
        }
    }
}
