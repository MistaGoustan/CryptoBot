using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TCK.Bot;
using TCK.Bot.Binance;
using TCK.Bot.Options;

namespace TCK.Exchanges.Binance
{
    public class CacheBinanceSpotMarketConnection : IBinanceSpotMarketConnection
    {
        private readonly IMemoryCache _cache;
        private readonly short _expireTimeInSeconds;
        private readonly IBinanceSpotMarketConnection _innerSpotMarket;

        public CacheBinanceSpotMarketConnection(IMemoryCache cache, IBinanceSpotMarketConnection innerSpotMarket, IOptions<ConfigurationOptions> options)
        {
            _cache = cache;
            _innerSpotMarket = innerSpotMarket;
            _expireTimeInSeconds = options.Value.ExpireTimeInSeconds;
        }

        public async Task<decimal> GetAvailableBalanceAsync(string tickerHalf)
        {
            return await _innerSpotMarket.GetAvailableBalanceAsync(tickerHalf);
        }

        public async Task<decimal> GetAvgPriceAsync(string ticker)
        {
            var key = $"Binance-{nameof(GetAvgPriceAsync)}-{ticker}";
            var balance = _cache.Get<decimal>(key);

            if (balance is 0)
            {
                balance = await _innerSpotMarket.GetAvgPriceAsync(ticker);

                _cache.Set(key, balance, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }

            return balance;
        }

        public async Task<int> GetBaseAssetPrecisionAsync(string ticker)
        {
            var key = $"Binance-{nameof(GetBaseAssetPrecisionAsync)}-{ticker}";
            var precision = _cache.Get<int>(key);

            if (precision is 0)
            {
                precision = await _innerSpotMarket.GetBaseAssetPrecisionAsync(ticker);

                _cache.Set(key, precision, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }

            return precision;
        }

        public async Task<SymbolLotSizeFilter> GetLotSizeAsync(string tickerLeft)
        {
            var key = $"Binance-{nameof(GetLotSizeAsync)}-{tickerLeft}";
            var lotSizeFilter = _cache.Get<SymbolLotSizeFilter>(key);

            if (lotSizeFilter is null)
            {
                lotSizeFilter = await _innerSpotMarket.GetLotSizeAsync(tickerLeft);

                _cache.Set(key, lotSizeFilter, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }

            return lotSizeFilter;
        }

        public async Task<SymbolPriceFilter> GetPriceFilterAsync(string ticker)
        {
            var key = $"Binance-{nameof(GetPriceFilterAsync)}-{ticker}";
            var priceFilter = _cache.Get<SymbolPriceFilter>(key);

            if (priceFilter is null)
            {
                priceFilter = await _innerSpotMarket.GetPriceFilterAsync(ticker);

                _cache.Set(key, priceFilter, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }

            return priceFilter;
        }

        public async Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(string ticker)
        {
            var key = $"Binance-{nameof(GetPricePercentFilterAsync)}-{ticker}";
            var pricePercentFilter = _cache.Get<SymbolPercentPriceFilter>(key);

            if (pricePercentFilter is null)
            {
                pricePercentFilter = await _innerSpotMarket.GetPricePercentFilterAsync(ticker);

                _cache.Set(key, pricePercentFilter, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }

            return pricePercentFilter;
        }

        public async Task<bool> TickerPairExistsAsync(string ticker)
        {
            var key = $"Binance-{nameof(TickerPairExistsAsync)}-{ticker}";
            var isExisting = _cache.Get<bool>(key);

            if (isExisting is false)
            {
                isExisting = await _innerSpotMarket.TickerPairExistsAsync(ticker);

                _cache.Set(key, isExisting, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }

            return isExisting;
        }
    }
}
