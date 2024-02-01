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

        public Task<Decimal> GetAvailableBalanceAsync(Exchange exchange, String tickerHalf)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetAvailableBalanceAsync(tickerHalf);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<Decimal> GetBalanceAsync(Exchange exchange, String tickerHalf)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<Decimal> GetAvgPriceAsync(Exchange exchange, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetAvgPriceAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<Int32> GetBaseAssetPrecisionAsync(Exchange exchange, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetBaseAssetPrecisionAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public async Task<Decimal> GetTotalFundingFeeAsync(Exchange exchange, DateTime startTime, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<SymbolLotSizeFilter> GetLotSizeAsync(Exchange exchange, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetLotSizeAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<SymbolPriceFilter> GetPriceFilterAsync(Exchange exchange, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetPriceFilterAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(Exchange exchange, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    return _binance.GetPricePercentFilterAsync(ticker);

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public async Task<Trade[]> GetTradesAsync(Exchange exchange, String ticker)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(exchange));
            }
        }

        public Task<Boolean> TickerPairExistsAsync(Exchange exchange, String ticker)
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
