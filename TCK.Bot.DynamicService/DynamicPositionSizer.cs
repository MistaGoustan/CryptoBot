using Microsoft.Extensions.Options;
using TCK.Bot.Extensions;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    public class DynamicPositionSizer : IDynamicPositionSizer
    {
        private readonly Decimal _riskPercent;
        private readonly IMarketConnection _market;

        public DynamicPositionSizer(IOptions<ConfigurationOptions> options, IMarketConnection market)
        {
            _riskPercent = options.Value.DynamicRiskPercent;
            _market = market;
        }

        public async Task<Decimal> GetDynamicBuySizeAsync(Decimal accountBalance, Decimal averagePrice, Exchange exchange, Decimal stopPrice, String ticker)
        {
            if (accountBalance is 0)
            {
                accountBalance = await _market.GetBalanceAsync(exchange, ticker.ToTickerRight());
            }

            var riskSize = GetRiskSize(accountBalance);

            var stopPercent = GetStopPercent(averagePrice, stopPrice);

            var orderSize = riskSize / stopPercent;

            var size = orderSize / averagePrice;

            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return size.ToStepSize(lotSize);
        }

        private Decimal GetRiskSize(Decimal size) =>
            size * _riskPercent;

        private Decimal GetStopPercent(Decimal averagePrice, Decimal stopPrice) =>
            Math.Abs(averagePrice - stopPrice) / averagePrice;
    }
}
