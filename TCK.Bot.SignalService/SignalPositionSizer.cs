using Microsoft.Extensions.Options;
using TCK.Bot.Extensions;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.SignalService
{
    public class SignalPositionSizer : ISignalPositionSizer
    {
        private readonly Decimal _riskPercent;
        private readonly IMarketConnection _market;

        public SignalPositionSizer(IOptions<ConfigurationOptions> options, IMarketConnection market)
        {
            _riskPercent = options.Value.SignalRiskPercent;
            _market = market;
        }

        public async Task<Decimal> GetSignalBuySizeAsync(Exchange exchange, Decimal stopPercent, String ticker)
        {
            var accountSize = await _market.GetBalanceAsync(exchange, ticker.ToTickerRight());

            var riskSize = GetRiskSize(accountSize);

            var orderSize = riskSize / stopPercent;

            var size = orderSize / await _market.GetAvgPriceAsync(exchange, ticker);

            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return size.ToStepSize(lotSize);
        }

        public async Task<Decimal> GetSignalSellSizeAsync(Exchange exchange, String ticker, String interval)
        {
            var balance = await _market.GetBalanceAsync(exchange, ticker.ToTickerLeft());
            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return balance.ToStepSize(lotSize);
        }

        private Decimal GetRiskSize(Decimal size) =>
            size * _riskPercent;
    }
}
