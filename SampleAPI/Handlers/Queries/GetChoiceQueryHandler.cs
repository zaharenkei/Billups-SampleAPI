using SampleAPI.Services.DataProviders;
using SampleAPI.Services.DataProviders.Models;
using SampleAPI.Services.ExternalClients;

namespace SampleAPI.Handlers.Queries
{
    /// <summary>
    /// Returns random choice from data provider.
    /// </summary>
    public class GetChoiceQueryHandler : IHandler<Choice>
    {
        private readonly IGameChoicesProvider dataProvider;
        private readonly IRandomClient randomClient;

        public GetChoiceQueryHandler(
            IGameChoicesProvider dataProvider,
            IRandomClient randomClient)
        {
            this.dataProvider = dataProvider;
            this.randomClient = randomClient;
        }

        /// <summary>
        /// Returns random choice from data provider.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Single choice obtained by random id.</returns>
        public async ValueTask<Choice> Execute(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var randomNumber = await randomClient.GetRandomNumber(token);
            var itemsAmount = await dataProvider.Count(token);
            var id = (randomNumber.Value % itemsAmount) + 1;

            return await dataProvider.Read(id, token);
        }
    }
}
