
namespace TCK.Bot.Services
{
    public interface IIsolatedWalletService
    {
        Task<Decimal> GetDynamicWalletBalanceAsync(Exchange exchange, String ticker);
        Task<Decimal> GetSignalWalletBalanceAsync(Exchange exchange, String ticker);
        Task<DynamicIsolatedWallet> UpdateDynamicWalletAsync(DynamicOrder order, OrderSide orderSide);
        Task UpdateSignalWalletAsync(SignalOrder order, OrderSide side);
    }
}