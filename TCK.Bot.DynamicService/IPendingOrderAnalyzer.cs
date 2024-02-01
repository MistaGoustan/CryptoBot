namespace TCK.Bot.DynamicService
{
    public interface IPendingOrderAnalyzer
    {
        Task<Boolean> ForPriceWithTickerAsync(Decimal lastPrice, DynamicOrder order);
    }
}