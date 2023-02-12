namespace SampleAPI.Services.DataProviders.Models
{
    public class LimitedQueue<TEntity> : Queue<TEntity>
    {
        private readonly int limit;

        public LimitedQueue(int limit) : base(limit)
        {
            this.limit = limit;
        }

        /// <summary>
        /// Overload to auto clean-up old records when new were added.
        /// </summary>
        /// <param name="item">Item to enqueue.</param>
        public new void Enqueue(TEntity item)
        {
            while (Count >= limit)
            {
                Dequeue();
            }
            base.Enqueue(item);
        }
    }
}
