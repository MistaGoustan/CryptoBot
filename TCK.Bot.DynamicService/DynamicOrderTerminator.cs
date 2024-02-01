using TCK.Bot.Data;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    internal class DynamicOrderTerminator : IDynamicOrderTerminator
    {
        private readonly IMarketConnection _market;
        private readonly IDynamicOrderService _orderService;
        private readonly IDynamicPNLCalculator _pnl;
        private readonly IDynamicOrderRepository _repository;

        public DynamicOrderTerminator(IMarketConnection market,
                                      IDynamicOrderService orderService,
                                      IDynamicPNLCalculator pnl,
                                      IDynamicOrderRepository repository)
        {
            _market = market;
            _orderService = orderService;
            _pnl = pnl;
            _repository = repository;
        }

        public async Task<DynamicOrder[]> ForAsync(Exchange exchange, DynamicOrder[] orders)
        {
            for (Int16 i = 0; i < orders.Length; i++)
            {
                if (orders[i].Status is DynamicOrderStatus.Pending)
                {
                    orders[i].Status = DynamicOrderStatus.Canceled;
                }
                else if (orders[i].Status is DynamicOrderStatus.InProgress)
                {
                    var price = await _market.GetAvgPriceAsync(exchange, orders[i].Ticker);

                    var placedOrder = orders[i].PositionSide is PositionSide.Long ?
                        await _orderService.PlaceMarketSellAsync(exchange, price, orders[i].TargetQuantity, orders[i].PositionSide, orders[i].Ticker) :
                        await _orderService.PlaceMarketBuyAsync(exchange, price, orders[i].TargetQuantity, orders[i].PositionSide, orders[i].Ticker);

                    orders[i].SellDate = DateTime.UtcNow;
                    orders[i].SellFee = placedOrder.Fee;
                    orders[i].SellOrderId = placedOrder.OrderId;
                    orders[i].SellPrice = placedOrder.Price;
                    orders[i].Status = DynamicOrderStatus.Completed;
                }
            }

            await _pnl.ForOrdersAsync(orders);

            orders = await _repository.UpdateOrdersAsync(orders, OrderSide.Sell);

            return orders;
        }
    }
}
