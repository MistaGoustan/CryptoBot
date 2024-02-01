namespace TCK.Bot.Data
{
    public interface IDynamicIsolatedWalletRepository
    {
        void DeleteWalletWithTicker(String ticker);
        Task<DynamicIsolatedWallet> GetWalletAsync(Exchange exchange, String ticker);
        DynamicIsolatedWallet UpdateWallet(DynamicIsolatedWallet wallet);
    }
}
