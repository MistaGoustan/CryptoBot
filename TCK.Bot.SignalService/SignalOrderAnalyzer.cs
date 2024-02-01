using TCK.Bot.Data;
using TCK.Bot.Services;

namespace TCK.Bot.SignalService
{
    public class SignalOrderAnalyzer : ISignalOrderAnalyzer
    {
        private readonly ICacheManager _cache;
        private readonly ISignalOrderRepository _orderRepository;
        private readonly ISignalOrderService _orderService;

        public SignalOrderAnalyzer(ICacheManager cache, ISignalOrderRepository orderRepository, ISignalOrderService orderService)
        {
            _cache = cache;
            _orderRepository = orderRepository;
            _orderService = orderService;
        }

        public async Task ForPriceWithTickerAsync(Exchange exchange, Decimal lastPrice, String ticker)
        {
            var order = _cache.GetSignalOrder(exchange, ticker);

            if (order is null) return;

            if (HasHitStopPrice(lastPrice, order.PositionSide, order.StopPrice))
            {
                order = await TriggerSellAsync(order, order.StopPrice);

                _orderRepository.UpdateOrder(order, OrderSide.Sell);
                _cache.RemoveSignalOrder(order.Exchange, order.Ticker);

                Console.WriteLine($"{nameof(SignalOrder)} - StopPrice hit for {order.Ticker} on {order.Exchange}");
            }
        }

        private Boolean HasHitStopPrice(Decimal lastPrice, PositionSide side, Decimal stopPrice) =>
            side is PositionSide.Long ? lastPrice < stopPrice : lastPrice > stopPrice;

        private async Task<SignalOrder> TriggerSellAsync(SignalOrder order, Decimal sellPrice)
        {
            var placedOrder = order.PositionSide is PositionSide.Long ?
                        await _orderService.PlaceMarketSellAsync(order.Exchange, sellPrice, order.Quantity, order.PositionSide, order.Ticker) :
                        await _orderService.PlaceMarketBuyAsync(order.Exchange, sellPrice, order.Quantity, order.PositionSide, order.Ticker);

            order.SellOrderId = placedOrder.OrderId;
            order.SellDate = DateTime.UtcNow;
            order.SellFee = placedOrder.Fee;
            order.SellPrice = placedOrder.Price;
            order.Status = SignalOrderStatus.Completed;

            return order;
        }
    }
}
