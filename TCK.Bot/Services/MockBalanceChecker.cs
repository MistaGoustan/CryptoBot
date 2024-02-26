using Microsoft.Extensions.Options;
using TCK.Bot.Options;

namespace TCK.Bot.Services
{
    internal class MockBalanceChecker : IBalanceChecker
    {
        private readonly IBalanceChecker _innerChecker;
        private readonly bool _checkBalance;

        public MockBalanceChecker(IBalanceChecker innerChecker, IOptions<BinanceOptions> config)
        {
            _innerChecker = innerChecker;
            _checkBalance = config.Value.CheckBalance;
        }

        public async Task<bool> HasEnoughInAccountAsync(string baseAsset, Exchange exchange, bool isWeighted, DynamicOrder[] uncahcedOrders)
        {
            if (exchange is Exchange.Binance && _checkBalance is false)
            {
                return true;
            }

            return await _innerChecker.HasEnoughInAccountAsync(baseAsset, exchange, isWeighted, uncahcedOrders);
        }
    }
}
