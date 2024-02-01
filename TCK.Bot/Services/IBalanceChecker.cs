namespace TCK.Bot.Services
{
    public interface IBalanceChecker
    {
        Task<Boolean> HasEnoughInAccountAsync(String baseAsset, Exchange exchange, Boolean isWeighted, DynamicOrder[] uncahcedOrders);
    }
}