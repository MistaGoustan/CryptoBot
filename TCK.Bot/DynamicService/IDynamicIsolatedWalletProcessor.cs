namespace TCK.Bot.DynamicService
{
    public interface IDynamicIsolatedWalletProcessor
    {
        Task<DynamicIsolatedWallet> ForBuyOrderAsync(DynamicOrder order);
        Task<DynamicIsolatedWallet> ForSellOrderAsync(DynamicOrder order);
    }
}