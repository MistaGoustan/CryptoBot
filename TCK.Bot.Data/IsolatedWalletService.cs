using TCK.Bot.DynamicService;
using TCK.Bot.Extensions;
using TCK.Bot.Services;

namespace TCK.Bot.Data
{
    internal class IsolatedWalletService : IIsolatedWalletService
    {
        private readonly IDynamicIsolatedWalletProcessor _dynamicWalletProcessor;
        private readonly IDynamicIsolatedWalletRepository _dynamicRepo;
        private readonly ISignalIsolatedWalletRepository _signalRepo;

        public IsolatedWalletService(IDynamicIsolatedWalletProcessor dynamicWalletProcessor, IDynamicIsolatedWalletRepository dynamicRepo, ISignalIsolatedWalletRepository signalWallet)
        {
            _dynamicWalletProcessor = dynamicWalletProcessor;
            _dynamicRepo = dynamicRepo;
            _signalRepo = signalWallet;
        }

        public async Task<decimal> GetDynamicWalletBalanceAsync(Exchange exchange, string ticker)
        {
            return (await _dynamicRepo.GetWalletAsync(exchange, ticker)).Balance;
        }

        public async Task<decimal> GetSignalWalletBalanceAsync(Exchange exchange, string ticker)
        {
            return await _signalRepo.GetBalanceAsync(exchange, ticker);
        }

        public async Task<DynamicIsolatedWallet> UpdateDynamicWalletAsync(DynamicOrder order, OrderSide orderSide)
        {
            var wallet = orderSide == OrderSide.Buy ?
                await _dynamicWalletProcessor.ForBuyOrderAsync(order) :
                await _dynamicWalletProcessor.ForSellOrderAsync(order);

            return _dynamicRepo.UpdateWallet(wallet);
        }

        public async Task UpdateSignalWalletAsync(SignalOrder order, OrderSide side)
        {
            var price = side is OrderSide.Buy ? order.BuyPrice : order.SellPrice;

            var orderBaseSize = order.Quantity.ToSize(price);

            var currentBalance = await _signalRepo.GetBalanceAsync(order.Exchange, order.Ticker);
            var newBalance = CalculateNewBalance(currentBalance, (order.BuyFee + order.SellFee), orderBaseSize, side);

            await _signalRepo.UpdateBalanceAsync(order.Ticker, newBalance, price);
        }

        private static decimal CalculateNewBalance(decimal currentBalance, decimal fee, decimal orderBaseSize, OrderSide orderSide)
        {
            return orderSide switch
            {
                OrderSide.Buy => currentBalance - orderBaseSize,
                OrderSide.Sell => currentBalance + orderBaseSize - fee,
                _ => throw new ArgumentOutOfRangeException(nameof(orderSide)),
            };
        }
    }
}
