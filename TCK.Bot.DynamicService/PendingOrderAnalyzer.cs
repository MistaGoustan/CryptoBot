using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TCK.Bot.Options;

namespace TCK.Bot.DynamicService
{
    internal class PendingOrderAnalyzer : IPendingOrderAnalyzer
    {
        private readonly Boolean _isProduction;
        private readonly ILogger<PendingOrderAnalyzer> _logger;
        private readonly IDynamicOrderService _orderService;

        public PendingOrderAnalyzer(IOptions<ConfigurationOptions> configuration,
                                    ILogger<PendingOrderAnalyzer> logger,
                                    IDynamicOrderService orderService)
        {
            _isProduction = configuration.Value.IsProduction;
            _logger = logger;
            _orderService = orderService;
        }

        public async Task<Boolean> ForPriceWithTickerAsync(Decimal lastPrice, DynamicOrder order)
        {
            if (HasHitBuyPrice(lastPrice, order.PositionSide, order.BuyPrice))
            {
                var placedOrder = order.PositionSide is PositionSide.Long ?
                    await _orderService.PlaceMarketBuyAsync(order.Exchange, order.BuyPrice, order.QuantityQuoted, order.PositionSide, order.Ticker) :
                    await _orderService.PlaceMarketSellAsync(order.Exchange, order.BuyPrice, order.QuantityQuoted, order.PositionSide, order.Ticker);

                order.BuyPrice = placedOrder.Price;
                order.BuyDate = DateTime.UtcNow;
                order.BuyFee = placedOrder.Fee;
                order.BuyOrderId = placedOrder.OrderId;
                order.QuantityFilled = placedOrder.Quantity;
                order.Status = DynamicOrderStatus.InProgress;

                _logger.LogInformation($"{nameof(DynamicOrder)} - BuyPrice hit for {order.Ticker} on {order.Exchange}");

                return true;
            }

            return false;
        }

        private Boolean HasHitBuyPrice(Decimal lastPrice, PositionSide side, Decimal buyPrice) =>
            side is PositionSide.Long ? lastPrice <= buyPrice : lastPrice >= buyPrice;
    }
}
