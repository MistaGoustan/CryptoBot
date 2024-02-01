namespace TCK.Bot.DynamicService
{
    public interface IInProgressOrderAnalyzer
    {
        Task<Boolean> ForPriceWithTickerAsync(Decimal lastPrice, DynamicOrder order);
    }
}