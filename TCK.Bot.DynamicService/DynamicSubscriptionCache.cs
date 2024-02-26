using Microsoft.Extensions.Logging;

namespace TCK.Bot.DynamicService
{
    public class DynamicSubscriptionCache : IDynamicSubscriptionCache
    {
        private readonly ILogger<DynamicSubscriptionCache> _logger;

        private List<Subscription> subscriptions = new();

        public DynamicSubscriptionCache(ILogger<DynamicSubscriptionCache> logger)
        {
            _logger = logger;
        }

        public void Add(Subscription subscription)
        {
            if (subscriptions.Any(s => s.Ticker == subscription.Ticker))
            {
                _logger.LogWarning($"Already subscribed to {subscription.Ticker}");

                return;
            }

            subscriptions.Add(subscription);
        }

        public Subscription GetOrDefault(string ticker)
        {
            var subscription = subscriptions.FirstOrDefault(s => s.Ticker == ticker)
                ?? throw new Exception("Subscription does not exist");

            return subscription;
        }

        public List<Subscription> GetAll() =>
            subscriptions;

        public void Remove(Subscription subscription) =>
            subscriptions.Remove(subscription);
    }
}
