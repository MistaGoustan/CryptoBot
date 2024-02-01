namespace TCK.Bot.DynamicService
{
    public interface IDynamicOrderAnalyzer
    {
        Task ForPriceAsync(Decimal lastPrice, DynamicOrder[] orders);
    }
}