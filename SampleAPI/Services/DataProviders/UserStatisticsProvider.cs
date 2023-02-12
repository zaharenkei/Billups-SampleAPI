using System.Collections.Concurrent;
using SampleAPI.Infrastructure.Exceptions;
using SampleAPI.Services.DataProviders.Models;

namespace SampleAPI.Services.DataProviders
{
    public class UserStatisticsProvider : IUserStatisticsProvider
    {
        private const int DefaultQueueLimit = 10;
        private readonly ConcurrentDictionary<string, LimitedQueue<RoundScore>> statisticsStorage = new();

        /// <inheritdocs/>
        public ValueTask<bool> Exist(string id, CancellationToken token)
        {
            var exist = statisticsStorage.ContainsKey(id);
            return ValueTask.FromResult(exist);
        }

        /// <inheritdocs/>
        public ValueTask<int> Count(CancellationToken token)
        {
            var amount = statisticsStorage.Count;
            return ValueTask.FromResult(amount);
        }

        /// <inheritdocs/>
        public ValueTask<IEnumerable<RoundScore>> CreateOrUpdate(string id, RoundScore roundScore, CancellationToken token)
        {
            var queue = statisticsStorage.AddOrUpdate(id,
                (_) =>
                {
                    var queue = new LimitedQueue<RoundScore>(DefaultQueueLimit);
                    queue.Enqueue(roundScore);
                    return queue;
                },
                (_, oldQueue) =>
                {
                    oldQueue.Enqueue(roundScore);
                    return oldQueue;
                });

            return ValueTask.FromResult(queue.ToArray() as IEnumerable<RoundScore>);
        }

        /// <inheritdocs/>
        public ValueTask<IEnumerable<RoundScore>> Read(string id, CancellationToken token)
        {
            if (!statisticsStorage.TryGetValue(id, out var queue))
            {
                statisticsStorage.TryAdd(id, new LimitedQueue<RoundScore>(DefaultQueueLimit));
                statisticsStorage.TryGetValue(id, out queue);
            }

            return ValueTask.FromResult(queue!.ToArray() as IEnumerable<RoundScore>);
        }

        /// <inheritdocs/>
        public ValueTask<bool> Delete(string id, CancellationToken token)
        {
            statisticsStorage.AddOrUpdate(id,
                (_) => new LimitedQueue<RoundScore>(DefaultQueueLimit),
                (_, oldQueue) =>
                {
                    oldQueue.Clear();
                    return oldQueue;
                });

            return ValueTask.FromResult(true);
        }
    }
}
