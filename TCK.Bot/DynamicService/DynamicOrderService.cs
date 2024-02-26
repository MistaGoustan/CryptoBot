using TCK.Bot.Binance;

namespace TCK.Bot.DynamicService
{
    public sealed class DynamicOrderService : IDynamicOrderService
    {
        private readonly IBinanceOrderService _binance;

        public DynamicOrderService(IBinanceOrderService binance)
        {
            _binance = binance;
        }

        public async Task<PlacedOrder> PlaceLimitBuyAsync(Exchange exchange, decimal price, decimal quantity, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return await _binance.PlaceLimitBuyAsync(ticker, quantity, price);

                default:
                    throw new ArgumentOutOfRangeException($"No option to place order on {exchange}.");
            }
        }

        public async Task<PlacedOrder> PlaceLimitSellAsync(Exchange exchange, decimal price, decimal quantity, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return await _binance.PlaceLimitSellAsync(ticker, quantity, price);

                default:
                    throw new ArgumentOutOfRangeException($"No option to place order on {exchange}.");
            }
        }

        public async Task<PlacedOrder> PlaceMarketBuyAsync(Exchange exchange, decimal price, decimal quantity, PositionSide side, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return await _binance.PlaceMarketBuyAsync(price, quantity, ticker);

                default:
                    throw new ArgumentOutOfRangeException($"No option to place order on {exchange}.");
            }
        }

        public async Task<PlacedOrder> PlaceMarketSellAsync(Exchange exchange, decimal price, decimal quantity, PositionSide side, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return await _binance.PlaceMarketSellAsync(price, quantity, ticker);

                default:
                    throw new ArgumentOutOfRangeException($"No option to place order on {exchange}.");
            }
        }
    }
}
