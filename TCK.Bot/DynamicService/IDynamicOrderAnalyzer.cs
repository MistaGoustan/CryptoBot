namespace TCK.Bot.DynamicService
{
    public interface IDynamicOrderAnalyzer
    {
        Task ForPriceAsync(decimal lastPrice, DynamicOrder[] orders);
    }
}