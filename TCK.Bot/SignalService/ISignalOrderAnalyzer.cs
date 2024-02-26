namespace TCK.Bot.SignalService
{
    public interface ISignalOrderAnalyzer
    {
        public Task ForPriceWithTickerAsync(Exchange exchange, decimal lastPrice, string ticker);
    }
}
