namespace TCK.Bot.Services
{
    public interface IMarketConnection
    {
        Task<Decimal> GetAvailableBalanceAsync(Exchange exchange, String tickerHalf);
        Task<Decimal> GetBalanceAsync(Exchange exchange, String tickerHalf);
        Task<Decimal> GetAvgPriceAsync(Exchange exchange, String ticker);
        Task<Int32> GetBaseAssetPrecisionAsync(Exchange exchange, String ticker);
        Task<SymbolLotSizeFilter> GetLotSizeAsync(Exchange exchange, String tickerLeft);
        Task<SymbolPriceFilter> GetPriceFilterAsync(Exchange exchange, String ticker);
        Task<SymbolPercentPriceFilter> GetPricePercentFilterAsync(Exchange exchange, String ticker);
        Task<Decimal> GetTotalFundingFeeAsync(Exchange exchange, DateTime startTime, String ticker);
        Task<Trade[]> GetTradesAsync(Exchange exchange, String ticker);
        Task<Boolean> TickerPairExistsAsync(Exchange exchange, String ticker);
    }
}
