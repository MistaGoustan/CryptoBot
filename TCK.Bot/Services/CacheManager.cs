using Microsoft.Extensions.Caching.Memory;

namespace TCK.Bot.Services
{
    internal class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;

        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public SignalOrder GetSignalOrder(Exchange exchange, string ticker)
        {
            var key = $"SignalOrders-{exchange}-{ticker}";

            return _cache.Get<SignalOrder>(key);
        }

        public void RemoveSignalOrder(Exchange exchange, string ticker)
        {
            var key = $"SignalOrders-{exchange}-{ticker}";

            _cache.Remove(key);
        }

        public void SetSignalOrder(SignalOrder order)
        {
            var key = $"SignalOrders-{order.Exchange}-{order.Ticker}";

            _cache.Set(key, order);
        }
    }
}