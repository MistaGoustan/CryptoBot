using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TCK.Bot.Options;

namespace TCK.Bot.DynamicService
{
    internal class InProgressOrderAnalyzer : IInProgressOrderAnalyzer
    {
        private readonly Boolean _isProduction;
        private readonly ILogger<InProgressOrderAnalyzer> _logger;
        private readonly IDynamicOrderService _orderService;
        public InProgressOrderAnalyzer(IOptions<ConfigurationOptions> configuration,
                                       ILogger<InProgressOrderAnalyzer> logger,
                                       IDynamicOrderService orderService)
        {
            _isProduction = configuration.Value.IsProduction;
            _logger = logger;
            _orderService = orderService;
        }

        public async Task<Boolean> ForPriceWithTickerAsync(Decimal lastPrice, DynamicOrder order)
        {
            if (HasHitTargetPrice(lastPrice, order.PositionSide, order.TargetPrice))
            {
                order = await TriggerSellAsync(order, order.TargetPrice);

                _logger.LogInformation($"{nameof(DynamicOrder)} - TargetPrice hit for {order.Ticker} on {order.Exchange}");

                return true;
            }

            if (HasHitStopPrice(lastPrice, order.PositionSide, order.StopPrice))
            {
                order = await TriggerSellAsync(order, order.StopPrice);

                _logger.LogInformation($"{nameof(DynamicOrder)} - StopPrice hit for {order.Ticker} on {order.Exchange}");

                return true;
            }

            return false;
        }

        private Boolean HasHitStopPrice(Decimal lastPrice, PositionSide side, Decimal stopPrice) =>
            side is PositionSide.Long ? lastPrice < stopPrice : lastPrice > stopPrice;

        private Boolean HasHitTargetPrice(Decimal lastPrice, PositionSide side, Decimal targetPrice) =>
            side is PositionSide.Long ? lastPrice > targetPrice : lastPrice < targetPrice;

        private async Task<DynamicOrder> TriggerSellAsync(DynamicOrder order, Decimal sellPrice)
        {
            var placedOrder = order.PositionSide is PositionSide.Long ?
                        await _orderService.PlaceMarketSellAsync(order.Exchange, sellPrice, order.QuantityQuoted, order.PositionSide, order.Ticker) :
                        await _orderService.PlaceMarketBuyAsync(order.Exchange, sellPrice, order.QuantityQuoted, order.PositionSide, order.Ticker);

            order.SellOrderId = placedOrder.OrderId;
            order.SellDate = DateTime.UtcNow;
            order.SellFee = placedOrder.Fee;
            order.SellPrice = placedOrder.Price;
            order.Status = DynamicOrderStatus.Completed;

            return order;
        }
    }
}