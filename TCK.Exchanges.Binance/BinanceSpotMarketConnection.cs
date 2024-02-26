using AutoMapper;
using Binance.Net.Objects.Models.Spot;
using Microsoft.Extensions.Options;
using TCK.Bot;
using TCK.Bot.Binance;
using TCK.Bot.Extensions;
using TCK.Bot.Options;

namespace TCK.Exchanges.Binance
{
    public sealed class BinanceSpotMarketConnection : BinanceBase, IBinanceSpotMarketConnection
    {
        private readonly IMapper _mapper;

        public BinanceSpotMarketConnection(IMapper mapper, IOptions<BinanceOptions> options) : base(options)
        {
            _mapper = mapper;
        }

        public async Task<decimal> GetAvgPriceAsync(string ticker)
        {
            var result = await Client.SpotApi.ExchangeData.GetCurrentAvgPriceAsync(ticker);
            return result.Data.Price;
        }

        public async Task<decimal> GetAvailableBalanceAsync(string tickerHalf)
        {
            var openOrders = await Client.SpotApi.Trading.GetOpenOrdersAsync();
            var unavailableBalance = 0m;

            foreach (var order in openOrders.Data)
            {
                if (order.Symbol.Contains(tickerHalf))
                {
                    unavailableBalance += tickerHalf.IsStableCoin()
                        ? order.QuantityRemaining * order.Price
                        : order.QuantityRemaining;
                }
            }

            var accountInfo = await Client.SpotApi.Account.GetAccountInfoAsync();
            var balance = accountInfo.Data.Balances.First(b => b.Asset == tickerHalf).Total - unavailableBalance;

            return balance;
        }

        public async Task<int> GetBaseAssetPrecisionAsync(string ticker)
        {
            var symbol = await GetSymbolAsync(ticker);

            return symbol.BaseAssetPrecision;
        }

        public async Task<SymbolLotSizeFilter> GetLotSizeAsync(string ticker)
        {
            var info = await Client.SpotApi.ExchangeData.GetExchangeInfoAsync(ticker);

            var symbol = info.Data.Symbols.SingleOrDefault() ?? throw new Exception($"More than one symbols found for ticker {ticker}");

            var filter = _mapper.Map<SymbolLotSizeFilter>(symbol.LotSizeFilter);

            return filter;
        }

        public async Task<SymbolPriceFilter> GetPriceFilterAsync(string ticker)
        {
            var symbol = await GetSymbolAsync(ticker);
            var binanceFilter = symbol.PriceFilter ?? throw new NullReferenceException($"PriceFilter does not exist for {ticker}");

            var filter = _mapper.Map<SymbolPriceFilter>(binanceFilter);

            return filter;
        }

        public async Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(string ticker)
        {
            var symbol = await GetSymbolAsync(ticker);
            var binanceFilter = symbol.PricePercentFilter ?? throw new NullReferenceException($"PricePercentFilter does not exist for {ticker}");

            var filter = _mapper.Map<SymbolPercentPriceFilter>(binanceFilter);

            return filter;
        }

        public async Task<bool> TickerPairExistsAsync(string ticker)
        {
            var result = await Client.SpotApi.ExchangeData.GetTickerAsync(ticker);
            return result.Data != null;
        }

        private async Task<BinanceSymbol> GetSymbolAsync(string ticker)
        {
            var exchangeInfo = await Client.SpotApi.ExchangeData.GetExchangeInfoAsync(ticker);
            var symbol = exchangeInfo.Data?.Symbols?.FirstOrDefault() ?? throw new Exception("Symbol does not exist");

            return symbol;
        }
    }
}
