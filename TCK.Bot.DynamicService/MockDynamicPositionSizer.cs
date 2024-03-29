﻿using Microsoft.Extensions.Options;
using TCK.Bot.Extensions;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    internal sealed class MockDynamicPositionSizer : IDynamicPositionSizer
    {
        private readonly IMarketConnection _market;
        private readonly IIsolatedWalletService _wallet;
        private readonly IDynamicPositionSizer _innerSizer;
        private readonly ConfigurationOptions _config;

        public MockDynamicPositionSizer(IMarketConnection market,
                                 IIsolatedWalletService wallet,
                                 IOptions<ConfigurationOptions> options,
                                 IDynamicPositionSizer innerSizer)
        {
            _market = market;
            _wallet = wallet;
            _innerSizer = innerSizer;
            _config = options.Value;
        }

        public async Task<decimal> GetDynamicBuySizeAsync(decimal accountbalance, decimal averagePrice, Exchange exchange, decimal stopPrice, string ticker)
        {
            if (_config.IsProduction)
            {
                return await _innerSizer.GetDynamicBuySizeAsync(accountbalance, averagePrice, exchange, stopPrice, ticker);
            }

            if (accountbalance is 0)
            {
                accountbalance = await _wallet.GetDynamicWalletBalanceAsync(exchange, ticker);
            }

            var riskSize = GetRiskSize(accountbalance);
            var stopPercent = GetStopPercent(averagePrice, stopPrice);

            var orderSize = riskSize / stopPercent;

            var size = orderSize / averagePrice;

            var lotSize = await _market.GetLotSizeAsync(exchange, ticker);

            return size.ToStepSize(lotSize);
        }

        private decimal GetRiskSize(decimal size) =>
            size * _config.DynamicRiskPercent;

        private decimal GetStopPercent(decimal averagePrice, decimal stopPrice) =>
            Math.Abs((averagePrice - stopPrice) / averagePrice);
    }
}
