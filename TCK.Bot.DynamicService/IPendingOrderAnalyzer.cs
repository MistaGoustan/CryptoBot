namespace TCK.Bot.DynamicService
{
    public interface IPendingOrderAnalyzer
    {
        Task<bool> ForPriceWithTickerAsync(decimal lastPrice, DynamicOrder order);
    }
}