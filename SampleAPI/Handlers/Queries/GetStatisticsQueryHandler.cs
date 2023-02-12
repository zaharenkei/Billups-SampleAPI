using SampleAPI.Handlers.Models;
using SampleAPI.Services.DataProviders;

namespace SampleAPI.Handlers.Queries
{
    public class GetStatisticsQueryHandler : IHandler<string, StatisticsResponse>
    {
        private readonly IUserStatisticsProvider dataProvider;

        public GetStatisticsQueryHandler(IUserStatisticsProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Reads the statistics based on a current connection.
        /// </summary>
        /// <param name="connectionId">Id of the connection as identifier.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns></returns>
        public async ValueTask<StatisticsResponse> Execute(string connectionId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var scores = await dataProvider.Read(connectionId, token);
            return new StatisticsResponse(scores);
        }
    }
}
