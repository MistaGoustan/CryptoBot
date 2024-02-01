using Binance.Net.Objects.Models.Spot;

namespace TCK.Exchanges.Binance
{
    public interface IBinanceFeeCalculator
    {
        Task<Decimal> GetFeeAsync(Decimal fee, String feeType, Decimal price);
        Task<Decimal> GetTotaledFeeAsync(IEnumerable<BinanceOrderTrade>? trades);
    }
}
