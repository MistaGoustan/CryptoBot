namespace TCK.Bot.SignalService
{
    public interface ISignalPositionSizer
    {
        Task<Decimal> GetSignalBuySizeAsync(Exchange exchange, Decimal stopPercent, String ticker);
        Task<Decimal> GetSignalSellSizeAsync(Exchange exchange, String ticker, String interval);
    }
}