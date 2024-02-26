namespace TCK.Bot.Services
{
    public interface IFuturesMarketConnection
    {
        Task<decimal> GetAvailableBalanceAsync(string tickerHalf);
        Task<decimal> GetAvgPriceAsync(string ticker);
        Task<int> GetBaseAssetPrecisionAsync(string ticker);
        Task<SymbolLotSizeFilter> GetLotSizeAsync(string tickerLeft);
        Task<SymbolPriceFilter> GetPriceFilterAsync(string ticker);
        Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(string ticker);
        Task<bool> TickerPairExistsAsync(string ticker);
    }
}
