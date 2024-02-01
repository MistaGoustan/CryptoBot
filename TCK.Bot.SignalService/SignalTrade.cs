using TCK.Bot.Api;
using TCK.Bot.Data;
using TCK.Bot.Services;

namespace TCK.Bot.SignalService
{
    public class SignalTrade : ISignalTrade
    {
        private readonly ICacheManager _cache;
        private readonly ISignalOrderService _orderService;
        private readonly ISignalOrderRepository _orderRepository;
        private readonly ISignalPositionSizer _positionSizer;
        private readonly ITickerSubscriber _tickerSubscriber;

        public SignalTrade(ICacheManager cache,
                           ISignalOrderService orderService,
                           ISignalOrderRepository orderRepository,
                           ISignalPositionSizer positionSizer,
                           ITickerSubscriber tickerSubscriber)
        {
            _cache = cache;
            _orderService = orderService;
            _orderRepository = orderRepository;
            _positionSizer = positionSizer;
            _tickerSubscriber = tickerSubscriber;
        }

        public async Task<IEnumerable<Object>?> GetRecentOrdersAsync(Exchange exchange, Boolean isDetailedTrades, Int16 numberOfOrders, String ticker)
        {
            var orders = await _orderRepository.GetRecentOrdersAsync(exchange, numberOfOrders, ticker);

            if (orders == null)
            {
                return null;
            }

            if (isDetailedTrades)
            {
                return orders;
            }

            var trades = new List<MiniSignalOrder>();

            foreach (var order in orders)
            {
                trades.Add(new MiniSignalOrder()
                {
                    BuyPrice = FormatDecimal(order.BuyPrice),
                    Fee = FormatDecimal(order.BuyFee + order.SellFee + order.FundingFee),
                    PNL = FormatDecimal(order.PNL ?? 0),
                    PositionSide = order.PositionSide,
                    Quantity = FormatDecimal(order.Quantity),
                    SellPrice = FormatDecimal(order.SellPrice),
                    Status = order.Status,
                    Ticker = order.Ticker
                });
            }

            return trades;
        }

        public async Task<SignalOrder> TradeAsync(SignalTradeRequest request)
        {
            var order = new SignalOrder();
            var placedOrder = new PlacedOrder();

            switch (request.OrderSide)
            {
                case OrderSide.Buy:
                    order = await CreateBuyOrderAsync(request);
                    placedOrder = await PlaceBuyOrderAsync(order, request.PositionSide);
                    order = ProcessBuyOrder(order, placedOrder);

                    order = _orderRepository.CreateOrder(order, request.OrderSide);
                    _cache.SetSignalOrder(order);

                    await _tickerSubscriber.SubscribeAsync(request.Exchange, request.Ticker, TradeType.Signal);

                    return order;

                case OrderSide.Sell:
                    order = await _orderRepository.GetInProgressOrderByIntervalAsync(request.Exchange, request.Interval, request.Ticker) ?? throw new Exception("Error: failed to sell. No order currently in progress.");
                    placedOrder = await PlaceSellOrderAsync(order, request.PositionSide);
                    order = ProcessSellOrder(order, placedOrder);

                    order = _orderRepository.UpdateOrder(order, request.OrderSide);
                    _cache.RemoveSignalOrder(order.Exchange, order.Ticker);

                    return order;

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private async Task<SignalOrder> CreateBuyOrderAsync(SignalTradeRequest request)
        {
            var avgPrice = 0; // TODO: await _market.GetAvgPriceAsync(request.Ticker);
            var stopPrice = CalculateStopPrice(request.PositionSide, avgPrice, request.StopPercent);

            return new SignalOrder
            {
                BuyDate = DateTime.UtcNow,
                Exchange = request.Exchange,
                Interval = request.Interval,
                PositionSide = request.PositionSide,
                Quantity = await _positionSizer.GetSignalBuySizeAsync(request.Exchange, request.StopPercent, request.Ticker),
                StopPrice = stopPrice,
                Ticker = request.Ticker
            };
        }

        private Decimal CalculateStopPrice(PositionSide side, Decimal price, Decimal stopPercent) =>
            side is PositionSide.Long ? price - price * stopPercent : price + price * stopPercent;

        private Decimal FormatDecimal(Decimal num) =>
            num < 1 ? Math.Round(num, 8) : Math.Round(num, 3);

        private async Task<PlacedOrder> PlaceBuyOrderAsync(SignalOrder order, PositionSide side) =>
            side is PositionSide.Long ?
                await _orderService.PlaceMarketBuyAsync(order.Exchange, order.BuyPrice, order.Quantity, order.PositionSide, order.Ticker) :
                await _orderService.PlaceMarketSellAsync(order.Exchange, order.BuyPrice, order.Quantity, order.PositionSide, order.Ticker);

        private async Task<PlacedOrder> PlaceSellOrderAsync(SignalOrder order, PositionSide side) =>
            side is PositionSide.Long ?
                await _orderService.PlaceMarketSellAsync(order.Exchange, order.SellPrice, order.Quantity, order.PositionSide, order.Ticker) :
                await _orderService.PlaceMarketBuyAsync(order.Exchange, order.SellPrice, order.Quantity, order.PositionSide, order.Ticker);

        private SignalOrder ProcessBuyOrder(SignalOrder order, PlacedOrder placedOrder)
        {
            order.BuyDate = DateTime.UtcNow;
            order.BuyOrderId = placedOrder.OrderId;
            order.BuyPrice = placedOrder.Price;
            order.BuyFee += placedOrder.Fee;
            order.Status = SignalOrderStatus.InProgress;

            return order;
        }

        private SignalOrder ProcessSellOrder(SignalOrder order, PlacedOrder placedOrder)
        {
            order.SellFee += placedOrder.Fee;
            order.SellDate = DateTime.UtcNow;
            order.SellOrderId = placedOrder.OrderId;
            order.SellPrice = placedOrder.Price;
            order.Status = SignalOrderStatus.Completed;

            return order;
        }
    }
}
