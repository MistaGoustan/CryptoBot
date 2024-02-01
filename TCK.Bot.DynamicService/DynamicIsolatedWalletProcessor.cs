using TCK.Bot.Data;
using TCK.Bot.Extensions;

namespace TCK.Bot.DynamicService
{
    public sealed class DynamicIsolatedWalletProcessor : IDynamicIsolatedWalletProcessor
    {
        private readonly IDynamicIsolatedWalletRepository _dynamicWallet;

        public DynamicIsolatedWalletProcessor(IDynamicIsolatedWalletRepository dynamicWallet)
        {
            _dynamicWallet = dynamicWallet;
        }

        public async Task<DynamicIsolatedWallet> ForBuyOrderAsync(DynamicOrder order)
        {
            var wallet = await _dynamicWallet.GetWalletAsync(order.Exchange, order.Ticker);

            var orderBaseSize = order.QuantityQuoted.ToSize(order.BuyPrice);

            var newBalance = wallet.AvailableBalance - orderBaseSize - order.BuyFee;

            wallet.AvailableBalance = newBalance;

            return wallet;
        }

        public async Task<DynamicIsolatedWallet> ForSellOrderAsync(DynamicOrder order)
        {
            var wallet = await _dynamicWallet.GetWalletAsync(order.Exchange, order.Ticker);

            var orderBaseSize = order.QuantityQuoted.ToSize(order.SellPrice);

            var newBalance = wallet.AvailableBalance + orderBaseSize - order.SellFee;

            wallet.AvailableBalance = newBalance;
            wallet.Balance = newBalance;

            return wallet;
        }
    }
}
