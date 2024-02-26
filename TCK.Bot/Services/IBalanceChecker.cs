namespace TCK.Bot.Services
{
    public interface IBalanceChecker
    {
        Task<bool> HasEnoughInAccountAsync(string baseAsset, Exchange exchange, bool isWeighted, DynamicOrder[] uncahcedOrders);
    }
}