using TCK.Bot.Api;
using TCK.Bot.Extensions;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    public class DynamicOrderFactory : IDynamicOrderFactory
    {
        private readonly ILotSizeRetriever _lotSizeRetriever;
        private readonly IDynamicPositionSizer _sizer;

        public DynamicOrderFactory(ILotSizeRetriever lotSizeRetriever, IDynamicPositionSizer sizer)
        {
            _lotSizeRetriever = lotSizeRetriever;
            _sizer = sizer;
        }

        public async Task<DynamicOrder[]> CreateEqualOrdersAsync(DynamicTradeRequest request)
        {
            var orders = InitializeOrders(request);

            orders = SetBuyPricesWithinRange(request.LowerBuyPrice, orders, request.PositionSide, request.UpperBuyPrice);
            orders = SetTargetPricesWithinRange(request.LowerTargetPrice, orders, request.PositionSide, request.UpperTargetPrice);

            var avgEntry = GetEqualAveragedEntry(orders);

            var size = await _sizer.GetDynamicBuySizeAsync(request.AccountBalance, avgEntry, request.Exchange, orders[0].StopPrice, request.Ticker);

            var filter = await _lotSizeRetriever.ForSymbolAsync(request.Exchange, request.Ticker);

            orders = SetEqualQuantities(orders, filter, size);

            return orders;
        }

        public async Task<DynamicOrder[]> CreateWeightedOrdersAsync(DynamicTradeRequest request)
        {
            var orders = InitializeOrders(request);

            orders = SetBuyPricesWithinRange(request.LowerBuyPrice, orders, request.PositionSide, request.UpperBuyPrice);
            orders = SetTargetPricesWithinRange(request.LowerTargetPrice, orders, request.PositionSide, request.UpperTargetPrice);

            var weightedAvgWithMod = GetWeightedAvgPriceWithMod(orders);

            var size = await _sizer.GetDynamicBuySizeAsync(request.AccountBalance, weightedAvgWithMod.AveragedEntry, request.Exchange, orders[0].StopPrice, request.Ticker);

            var filter = await _lotSizeRetriever.ForSymbolAsync(request.Exchange, request.Ticker);

            orders = SetWeightedQuantities(orders, filter, weightedAvgWithMod.Modifier, size);

            return orders;
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

        private DynamicOrder[] SetBuyPricesWithinRange(Decimal lowerBound, DynamicOrder[] orders, PositionSide side, Decimal upperBound)
        {
            if (side is PositionSide.Long)
            {
                for (Decimal i = 1; i <= orders.Length; i++)
                {
                    var modifier = i / orders.Length;
                    var price = (upperBound - lowerBound) * modifier + lowerBound;

                    orders[(Int16)i - 1].BuyPrice = Decimal.Round(price, 8);
                }
            }
            else
            {
                for (Decimal i = 0; i <= orders.Length - 1; i++)
                {
                    var modifier = i / orders.Length;

                    var price = i == 0 ?
                        lowerBound :
                        (upperBound - lowerBound) * modifier + lowerBound;

                    orders[(Int16)i].BuyPrice = Decimal.Round(price, 8);
                }
            }


            Array.Reverse(orders); // so the quantities and targets get put in the inverse order (first bought first sold)

            return orders;
        }

        private DynamicOrder[] SetEqualQuantities(DynamicOrder[] orders, SymbolLotSizeFilter filter, Decimal size)
        {
            var individualQuantitiy = size / orders.Length;
            individualQuantitiy = individualQuantitiy.ToStepSize(filter);

            foreach (var order in orders)
            {
                order.QuantityQuoted = individualQuantitiy;
            }

            return orders;
        }

        private DynamicOrder[] SetTargetPricesWithinRange(Decimal lowerBound, DynamicOrder[] orders, PositionSide side, Decimal upperBound)
        {
            if (side is PositionSide.Long)
            {
                for (Decimal i = 0; i <= orders.Length - 1; i++)
                {
                    var modifier = i == 0 ? 0 : i / orders.Length;
                    var price = (upperBound - lowerBound) * modifier + lowerBound;

                    orders[(Int16)i].TargetPrice = price;
                }
            }
            else
            {
                for (Decimal i = 1; i <= orders.Length; i++)
                {
                    var modifier = i / orders.Length;

                    var price = (upperBound - lowerBound) * modifier + lowerBound;

                    orders[(Int16)i - 1].TargetPrice = Decimal.Round(price, 8);
                }
            }

            return orders;
        }

        private DynamicOrder[] SetWeightedQuantities(DynamicOrder[] orders, SymbolLotSizeFilter filter, Decimal modifier, Decimal size)
        {
            for (Int16 i = 0; i < orders.Length; i++)
            {
                var position = i + 1;
                var currentQty = position / modifier * size;

                orders[i].QuantityQuoted = currentQty.ToStepSize(filter);
            }

            return orders;
        }

        private DynamicOrder[] InitializeOrders(DynamicTradeRequest request)
        {
            var orders = new DynamicOrder[request.NumberOfOrders];
            var orderGroupId = Guid.NewGuid().ToString();

            for (Int16 i = 0; i < orders.Length; i++)
            {
                orders[i] = new DynamicOrder
                {
                    OrderGroupId = orderGroupId,
                    PositionSide = request.PositionSide,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = request.StopPrice,
                    Ticker = request.Ticker
                };
            }

            return orders;
        }
    }
}
