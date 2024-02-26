namespace TCK.Bot.DynamicService
{
    public interface IInProgressOrderAnalyzer
    {
        Task<bool> ForPriceWithTickerAsync(decimal lastPrice, DynamicOrder order);
    }
}