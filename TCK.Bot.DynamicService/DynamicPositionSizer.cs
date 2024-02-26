using Microsoft.Extensions.Options;
using TCK.Bot.Extensions;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    public class DynamicPositionSizer : IDynamicPositionSizer
    {
        private readonly decimal _riskPercent;
        private readonly IMarketConnection _market;

        public DynamicPositionSizer(IOptions<ConfigurationOptions> options, IMarketConnection market)
        {
            _riskPercent = options.Value.DynamicRiskPercent;
            _market = market;
        }

        public async Task<decimal> GetDynamicBuySizeAsync(decimal accountBalance, decimal averagePrice, Exchange exchange, decimal stopPrice, string ticker)
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

        private decimal GetRiskSize(decimal size) =>
            size * _riskPercent;

        private decimal GetStopPercent(decimal averagePrice, decimal stopPrice) =>
            Math.Abs(averagePrice - stopPrice) / averagePrice;
    }
}
