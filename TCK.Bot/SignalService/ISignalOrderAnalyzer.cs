namespace TCK.Bot.SignalService
{
    public interface ISignalOrderAnalyzer
    {
        public Task ForPriceWithTickerAsync(Exchange exchange, Decimal lastPrice, String ticker);
    }
}
