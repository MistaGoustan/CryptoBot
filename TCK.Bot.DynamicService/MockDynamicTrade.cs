using Microsoft.Extensions.Options;
using TCK.Bot.Api;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    internal class MockDynamicTrade : IDynamicTrade
    {
        private readonly IDynamicOrderCache _cache;
        private readonly IDynamicTrade _innerTrader;
        private readonly Boolean _isProduction;
        private readonly IDynamicPNLCalculator _pnl;
        private readonly IDynamicOrderUpdater _updater;

        public MockDynamicTrade(IDynamicOrderCache cache,
                                IDynamicTrade innerTrader,
                                IOptions<ConfigurationOptions> options,
                                IDynamicPNLCalculator pnl,
                                IDynamicOrderUpdater updater)
        {
            _cache = cache;
            _innerTrader = innerTrader;
            _isProduction = options.Value.IsProduction;
            _pnl = pnl;
            _updater = updater;
        }

        public async Task<DynamicOrder[]?> CancelTradesAsync(CancelDynamicTradeRequest request)
        {
            var result = await _innerTrader.CancelTradesAsync(request);

            if (!_isProduction)
            {
                foreach (var ticker in request.Tickers)
                {
                    var orders = _cache.GetGroup(ticker);

                    await _pnl.ForOrdersAsync(orders);

                    await _updater.UpdateOrdersAsync(orders);

                    _cache.RemoveGroup(ticker);
                }
            }

            return result;
        }

        public async Task<DynamicOrder[]?> EditTradesAsync(EditDynamicTradeRequest request)
        {
            return await _innerTrader.EditTradesAsync(request);
        }

        public async Task<DynamicOrder[]> ExecuteTradesAsync(DynamicTradeRequest request)
        {
            var orders = await _innerTrader.ExecuteTradesAsync(request);

            if (!_isProduction)
            {
                await _pnl.ForOrdersAsync(orders);

                await _updater.UpdateOrdersAsync(orders);
            }

            return orders;
        }

        public async Task<IEnumerable<Object>?> GetTradesAsync(Exchange exchange, Boolean isDetailedTrades, String ticker)
        {
            return await _innerTrader.GetTradesAsync(exchange, isDetailedTrades, ticker);
        }
    }
}
