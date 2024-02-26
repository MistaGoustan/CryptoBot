namespace TCK.Bot.Binance
{
    public interface IBinanceSpotMarketConnection
    {
        Task<decimal> GetAvailableBalanceAsync(string tickerHalf);
        Task<decimal> GetAvgPriceAsync(string ticker);
        Task<int> GetBaseAssetPrecisionAsync(string ticker);
        Task<SymbolLotSizeFilter> GetLotSizeAsync(string ticker);
        Task<SymbolPriceFilter> GetPriceFilterAsync(string ticker);
        Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(string ticker);
        Task<bool> TickerPairExistsAsync(string ticker);
    }
}
