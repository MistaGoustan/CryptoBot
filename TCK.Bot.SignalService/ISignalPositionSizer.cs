namespace TCK.Bot.SignalService
{
    public interface ISignalPositionSizer
    {
        Task<decimal> GetSignalBuySizeAsync(Exchange exchange, decimal stopPercent, string ticker);
        Task<decimal> GetSignalSellSizeAsync(Exchange exchange, string ticker, string interval);
    }
}