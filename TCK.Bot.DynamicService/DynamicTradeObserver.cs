using TCK.Bot.Data;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    public class TradeObserver : ITradeObserver
    {
        private readonly IDynamicOrderCache _dynamicCache;
        private readonly IDynamicOrderRepository _dynamicRepository;
        private readonly ICacheManager _signalCache;
        private readonly ISignalOrderRepository _signalRepository;
        private readonly ITickerSubscriber _tickerSubscriber;

        public TradeObserver(IDynamicOrderCache dynamicCache,
                             IDynamicOrderRepository dynamicRepository,
                             ICacheManager signalCache,
                             ISignalOrderRepository signalRepository,
                             ITickerSubscriber tickerSubscriber)
        {
            _dynamicCache = dynamicCache;
            _dynamicRepository = dynamicRepository;
            _signalCache = signalCache;
            _signalRepository = signalRepository;
            _tickerSubscriber = tickerSubscriber;
        }

        public async Task InitializeAsync()
        {
            await ObserveDynamicOrdersAsync();
            await ObserveSignalOrdersAsync();
        }

        private async Task ObserveDynamicOrdersAsync()
        {
            var orders = await _dynamicRepository.GetUncompletedOrdersAsync();

            if (!orders.Any())
                return;

            var ordersByExchange = orders.GroupBy(o => o.Exchange);

            foreach (var groupedExchangeOrders in ordersByExchange)
            {
                var distinctTickers = groupedExchangeOrders.Select(g => g.Ticker).Distinct();

                foreach (var ticker in distinctTickers)
                {
                    _dynamicCache.AddGroup(orders);

                    await _tickerSubscriber.SubscribeAsync(groupedExchangeOrders.Key, ticker, TradeType.Dynamic);
                }
            }
        }

        private async Task ObserveSignalOrdersAsync()
        {
            var orders = await _signalRepository.GetInProgressOrders();

            if (orders is null || !orders.Any())
            {
                return;
            }

            foreach (var order in orders)
            {
                _signalCache.SetSignalOrder(order);

                await _tickerSubscriber.SubscribeAsync(order.Exchange, order.Ticker, TradeType.Signal);
            }
        }
    }
}
