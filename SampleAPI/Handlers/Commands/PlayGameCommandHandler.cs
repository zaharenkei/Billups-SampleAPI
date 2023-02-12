using SampleAPI.Handlers.Models;
using SampleAPI.Services.DataProviders;
using SampleAPI.Services.ExternalClients;
using SampleAPI.Services.ResultProvider;

namespace SampleAPI.Handlers.Commands
{
    /// <summary>
    /// Responds for player's choice with random one and returns the result.
    /// </summary>
    public class PlayGameCommandHandler : IHandler<PlayGameRequest, PlayGameResponse>
    {
        private readonly IGameChoicesProvider dataProvider;
        private readonly IRandomClient randomClient;
        private readonly IResultProvider resultProvider;
        private readonly IUserStatisticsProvider statisticsProvider;
        private readonly ILogger<PlayGameCommandHandler> logger;

        public PlayGameCommandHandler(
            IGameChoicesProvider dataProvider,
            IRandomClient randomClient,
            IResultProvider resultProvider,
            IUserStatisticsProvider statisticsProvider,
            ILogger<PlayGameCommandHandler> logger)
        {
            this.dataProvider = dataProvider;
            this.randomClient = randomClient;
            this.resultProvider = resultProvider;
            this.statisticsProvider = statisticsProvider;
            this.logger = logger;
        }

        /// <summary>
        /// Responds for player's choice with random one and returns the result.
        /// </summary>
        /// <param name="request">Player's request with specified id.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Response with game results.</returns>
        public async ValueTask<PlayGameResponse> Execute(PlayGameRequest request, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var randomNumber = await randomClient.GetRandomNumber(token);
            var itemsAmount = await dataProvider.Count(token);
            var computer = randomNumber.Value % itemsAmount + 1;

            var score = resultProvider.DefineWinner(request.Player, computer);

            if (request.ConnectionId != null)
            {
                await statisticsProvider.CreateOrUpdate(request.ConnectionId, score, token);
            }
            else
            {
                logger.LogWarning($"Statistics and some dependent services won't be available due to undefined connection.");
            }

            return new PlayGameResponse(score);
        }
    }
}
