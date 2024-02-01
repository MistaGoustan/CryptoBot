using Binance.Net.Objects.Models.Spot;
using TCK.Bot;
using TCK.Bot.Extensions;
using TCK.Bot.Services;

namespace TCK.Exchanges.Binance
{
    public class BinanceFeeCalculator : IBinanceFeeCalculator
    {
        private readonly IMarketConnection _market;

        public BinanceFeeCalculator(IMarketConnection market)
        {
            _market = market;
        }

        public async Task<Decimal> GetFeeAsync(Decimal fee, String feeType, Decimal price)
        {
            if (feeType.IsStableCoin())
            {
                return fee;
            }
            else if (feeType == "BNB")
            {
                price = await _market.GetAvgPriceAsync(Exchange.Binance, "BNBUSD");
            }

            return fee * price;
        }

        public async Task<Decimal> GetTotaledFeeAsync(IEnumerable<BinanceOrderTrade>? trades)
        {
            if (trades is null)
                throw new Exception("Cannot get fees from null trades.");

            var totaledFee = 0m;

            foreach (var trade in trades)
            {
                if (trade.FeeAsset.IsStableCoin())
                {
                    totaledFee += trade.Fee;
                }
                else
                {
                    var price = await GetPriceOfFeeType(trade.FeeAsset, trades);

                    totaledFee += trade.Fee * price;
                }
            }

            return totaledFee;
        }

        private static Decimal AveragePrice(IEnumerable<BinanceOrderTrade> trades)
        {
            var price = 0m;

            foreach (var trade in trades)
            {
                price += trade.Price;
            }

            return price / trades.Count();
        }

        private async Task<Decimal> GetPriceOfFeeType(String feeType, IEnumerable<BinanceOrderTrade> trades)
        {
            if (feeType == "BNB")
            {
                return await _market.GetAvgPriceAsync(Exchange.Binance, "BNBUSDT");
            }

            return AveragePrice(trades);
        }
    }
}
