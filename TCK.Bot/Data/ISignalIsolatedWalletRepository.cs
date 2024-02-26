namespace TCK.Bot.Data
{
    public interface ISignalIsolatedWalletRepository
    {
        void DeleteWalletWithTicker(string ticker);
        Task<decimal> GetBalanceAsync(Exchange exchange, string ticker);
        Task UpdateBalanceAsync(string ticker, decimal orderSize, decimal price);
    }
}
