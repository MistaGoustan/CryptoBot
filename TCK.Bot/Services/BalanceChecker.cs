using TCK.Bot.Binance;
using TCK.Bot.DynamicService;

namespace TCK.Bot.Services
{
    internal class BalanceChecker : IBalanceChecker
    {
        private readonly IBinanceSpotMarketConnection _binance;
        private readonly IDynamicOrderCache _cache;

        public BalanceChecker(IBinanceSpotMarketConnection binance, IDynamicOrderCache cache)
        {
            _binance = binance;
            _cache = cache;
        }

        public async Task<Boolean> HasEnoughInAccountAsync(String baseAsset, Exchange exchange, Boolean isWeighted, DynamicOrder[] uncahcedOrders)
        {
            var cachedPendingBalance = CalculateCachedPendingBalance(exchange);
            var uncachedPendingBalance = CalculateUncachedPendingBalance(isWeighted, uncahcedOrders);

            switch (exchange)
            {
                case Exchange.Binance:
                    return await DoesBinanceHaveEnoughAsync(baseAsset, cachedPendingBalance, uncachedPendingBalance);
                default:
                    throw new NotImplementedException();
            }
        }

        private Decimal CalculateCachedPendingBalance(Exchange exchange)
        {
            var groups = _cache.GetAllGroups();

            if (groups is null || !groups.Any())
                return 0;

            Decimal total = 0;

            foreach (var orders in groups)
            {
                if (orders[0].Exchange != exchange)
                    continue;

                var pendingOrders = orders.Where(o => o.Status is DynamicOrderStatus.Pending).ToArray();

                if (!pendingOrders.Any()) return 0;

                var totalQty = pendingOrders.Sum(o => o.QuantityQuoted);
                var avgQty = totalQty / pendingOrders.Count();

                var isWeighted = avgQty != pendingOrders[0].QuantityQuoted;

                total += CalculateUncachedPendingBalance(isWeighted, pendingOrders);
            }

            return total;
        }

        private Decimal CalculateUncachedPendingBalance(Boolean isWeighted, DynamicOrder[] orders)
        {
            var totalQty = orders.Sum(o => o.QuantityQuoted);
            var avgPrice = isWeighted ? GetWeightedAvgPriceWithMod(orders).AveragedEntry : GetEqualAveragedEntry(orders);

            return (totalQty * avgPrice);
        }

        private async Task<Boolean> DoesBinanceHaveEnoughAsync(String baseAsset, Decimal existingPendingBalance, Decimal newPendingBalance)
        {
            var availableExchangeBalance = await _binance.GetAvailableBalanceAsync(baseAsset);

            return availableExchangeBalance >= (newPendingBalance + existingPendingBalance);
        }

        private Decimal GetEqualAveragedEntry(DynamicOrder[] orders)
        {
            var averagedEntry = orders[0].BuyPrice;

            for (Int16 i = 0; i <= orders.Length - 1; i++)
            {
                if (i is 0)
                {
                    continue;
                }

                var sumOfCurrentQuantities = i + 1m;
                var priceDifference = orders[i].BuyPrice - averagedEntry;

                averagedEntry = priceDifference * (1m / sumOfCurrentQuantities) + averagedEntry;
            }

            return averagedEntry;
        }

        private (Decimal AveragedEntry, Decimal Modifier) GetWeightedAvgPriceWithMod(DynamicOrder[] orders)
        {
            var averagedEntry = orders[0].BuyPrice;
            var modifier = 1m;

            for (Int16 i = 0; i <= orders.Length - 1; i++)
            {
                if (i is 0)
                {
                    continue;
                }

                var priceDifference = orders[i].BuyPrice - averagedEntry;
                var position = i + 1;

                modifier = position + modifier;
                averagedEntry = priceDifference * (position / modifier) + averagedEntry;
            }

            return (averagedEntry, modifier);
        }
    }
}
