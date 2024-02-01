namespace TCK.Bot.DynamicService
{
    public interface IDynamicPositionSizer
    {
        Task<Decimal> GetDynamicBuySizeAsync(Decimal accountBalance, Decimal averagePrice, Exchange exchange, Decimal stopPrice, String ticker);
    }
}