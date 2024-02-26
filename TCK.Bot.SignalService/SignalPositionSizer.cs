using Microsoft.Extensions.Options;
using TCK.Bot.Extensions;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.SignalService
{
    public class SignalPositionSizer : ISignalPositionSizer
    {
        private readonly decimal _riskPercent;
        private readonly IMarketConnection _market;

        public SignalPositionSizer(IOptions<ConfigurationOptions> options, IMarketConnection market)
        {
            _riskPercent = options.Value.SignalRiskPercent;
            _market = market;
        }

        public async Task<decimal> GetSignalBuySizeAsync(Exchange exchange, decimal stopPercent, string ticker)
        {
            var accountSize = await _market.GetBalanceAsync(exchange, ticker.ToTickerRight());

            var riskSize = GetRiskSize(accountSize);

            var orderSize = riskSize / stopPercent;

            var size = orderSize / await _market.GetAvgPriceAsync(exchange, ticker);

            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return size.ToStepSize(lotSize);
        }

        public async Task<decimal> GetSignalSellSizeAsync(Exchange exchange, string ticker, string interval)
        {
            var balance = await _market.GetBalanceAsync(exchange, ticker.ToTickerLeft());
            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return balance.ToStepSize(lotSize);
        }

        private decimal GetRiskSize(decimal size) =>
            size * _riskPercent;
    }
}
