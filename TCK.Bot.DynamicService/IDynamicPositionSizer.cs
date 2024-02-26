namespace TCK.Bot.DynamicService
{
    public interface IDynamicPositionSizer
    {
        Task<decimal> GetDynamicBuySizeAsync(decimal accountBalance, decimal averagePrice, Exchange exchange, decimal stopPrice, string ticker);
    }
}