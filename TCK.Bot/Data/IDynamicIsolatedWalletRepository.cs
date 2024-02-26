namespace TCK.Bot.Data
{
    public interface IDynamicIsolatedWalletRepository
    {
        void DeleteWalletWithTicker(string ticker);
        Task<DynamicIsolatedWallet> GetWalletAsync(Exchange exchange, string ticker);
        DynamicIsolatedWallet UpdateWallet(DynamicIsolatedWallet wallet);
    }
}
