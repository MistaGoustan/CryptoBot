namespace TCK.Bot.Services
{
    public interface IFuturesMarketConnection
    {
        Task<Decimal> GetAvailableBalanceAsync(String tickerHalf);
        Task<Decimal> GetAvgPriceAsync(String ticker);
        Task<Int32> GetBaseAssetPrecisionAsync(String ticker);
        Task<SymbolLotSizeFilter> GetLotSizeAsync(String tickerLeft);
        Task<SymbolPriceFilter> GetPriceFilterAsync(String ticker);
        Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(String ticker);
        Task<Boolean> TickerPairExistsAsync(String ticker);
    }
}
