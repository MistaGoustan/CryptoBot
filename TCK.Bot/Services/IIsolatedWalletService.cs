
namespace TCK.Bot.Services
{
    public interface IIsolatedWalletService
    {
        Task<decimal> GetDynamicWalletBalanceAsync(Exchange exchange, string ticker);
        Task<decimal> GetSignalWalletBalanceAsync(Exchange exchange, string ticker);
        Task<DynamicIsolatedWallet> UpdateDynamicWalletAsync(DynamicOrder order, OrderSide orderSide);
        Task UpdateSignalWalletAsync(SignalOrder order, OrderSide side);
    }
}