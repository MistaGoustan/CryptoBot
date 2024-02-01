namespace TCK.Bot.Data
{
    public interface ISignalIsolatedWalletRepository
    {
        void DeleteWalletWithTicker(String ticker);
        Task<Decimal> GetBalanceAsync(Exchange exchange, String ticker);
        Task UpdateBalanceAsync(String ticker, Decimal orderSize, Decimal price);
    }
}
