using Microsoft.Extensions.Options;
using TCK.Bot.Data;
using TCK.Bot.Extensions;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.SignalService
{
    internal sealed class MockSignalPositionSizer : ISignalPositionSizer
    {
        private readonly IMarketConnection _market;
        private readonly IIsolatedWalletService _wallet;
        private readonly ISignalOrderRepository _orderRepository;
        private readonly ISignalPositionSizer _innerSizer;
        private readonly ConfigurationOptions _config;

        public MockSignalPositionSizer(IMarketConnection market,
                                 IIsolatedWalletService wallet,
                                 ISignalOrderRepository orderRepository,
                                 IOptions<ConfigurationOptions> options,
                                 ISignalPositionSizer innerSizer)
        {
            _market = market;
            _wallet = wallet;
            _orderRepository = orderRepository;
            _innerSizer = innerSizer;
            _config = options.Value;
        }

        public async Task<Decimal> GetSignalBuySizeAsync(Exchange exchange, Decimal stopPercent, String ticker)
        {
            if (_config.IsProduction)
            {
                return await _innerSizer.GetSignalBuySizeAsync(exchange, stopPercent, ticker);
            }

            var accountSize = await _wallet.GetSignalWalletBalanceAsync(exchange, ticker);

            var riskSize = GetRiskSize(accountSize);

            var orderSize = riskSize / stopPercent;

            var size = orderSize / await _market.GetAvgPriceAsync(exchange, ticker);

            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return size.ToStepSize(lotSize);
        }

        public async Task<Decimal> GetSignalSellSizeAsync(Exchange exchange, String ticker, String interval)
        {
            if (_config.IsProduction)
            {
                return await _innerSizer.GetSignalSellSizeAsync(exchange, ticker, interval);
            }

            var recentOrder = await _orderRepository.GetInProgressOrderByIntervalAsync(exchange, ticker, interval);

            return recentOrder.Quantity;
        }

        private Decimal GetRiskSize(Decimal size) =>
            size * _config.SignalRiskPercent;
    }
}
