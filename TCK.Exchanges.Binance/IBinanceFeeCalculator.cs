using Binance.Net.Objects.Models.Spot;

namespace TCK.Exchanges.Binance
{
    public interface IBinanceFeeCalculator
    {
        Task<decimal> GetFeeAsync(decimal fee, string feeType, decimal price);
        Task<decimal> GetTotaledFeeAsync(IEnumerable<BinanceOrderTrade>? trades);
    }
}
