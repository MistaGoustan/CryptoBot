using TCK.Bot.Binance;

namespace TCK.Bot.Services
{
    internal class MarketConnection : IMarketConnection
    {
        private readonly IBinanceSpotMarketConnection _binance;

        public MarketConnection(IBinanceSpotMarketConnection binance)
        {
            _binance = binance;
        }

        public Task<decimal> GetAvailableBalanceAsync(Exchange exchange, string tickerHalf)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetAvailableBalanceAsync(tickerHalf);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<decimal> GetBalanceAsync(Exchange exchange, string tickerHalf)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<decimal> GetAvgPriceAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetAvgPriceAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<int> GetBaseAssetPrecisionAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetBaseAssetPrecisionAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public async Task<decimal> GetTotalFundingFeeAsync(Exchange exchange, DateTime startTime, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<SymbolLotSizeFilter> GetLotSizeAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetLotSizeAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<SymbolPriceFilter> GetPriceFilterAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetPriceFilterAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetPricePercentFilterAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public async Task<Trade[]> GetTradesAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<bool> TickerPairExistsAsync(Exchange exchange, string ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.TickerPairExistsAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }
    }
}
