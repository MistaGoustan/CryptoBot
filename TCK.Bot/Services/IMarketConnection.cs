namespace TCK.Bot.Services
{
    public interface IMarketConnection
    {
        Task<decimal> GetAvailableBalanceAsync(Exchange exchange, string tickerHalf);
        Task<decimal> GetBalanceAsync(Exchange exchange, string tickerHalf);
        Task<decimal> GetAvgPriceAsync(Exchange exchange, string ticker);
        Task<int> GetBaseAssetPrecisionAsync(Exchange exchange, string ticker);
        Task<SymbolLotSizeFilter> GetLotSizeAsync(Exchange exchange, string tickerLeft);
        Task<SymbolPriceFilter> GetPriceFilterAsync(Exchange exchange, string ticker);
        Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(Exchange exchange, string ticker);
        Task<decimal> GetTotalFundingFeeAsync(Exchange exchange, DateTime startTime, string ticker);
        Task<Trade[]> GetTradesAsync(Exchange exchange, string ticker);
        Task<bool> TickerPairExistsAsync(Exchange exchange, string ticker);
    }
}
