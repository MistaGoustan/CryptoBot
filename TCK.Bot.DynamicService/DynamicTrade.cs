using TCK.Bot.Api;
using TCK.Bot.Data;
using TCK.Bot.Extensions;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    internal class DynamicTrade : IDynamicTrade
    {
        private readonly IBalanceChecker _balanceChecker;
        private readonly IDynamicOrderCache _cache;
        private readonly IDynamicOrderFactory _orderFactory;
        private readonly IDynamicOrderRepository _orderRepository;
        private readonly IDynamicOrderTerminator _terminator;
        private readonly ITickerSubscriber _tickerSubscriber;

        public DynamicTrade(IBalanceChecker balanceChecker,
                            IDynamicOrderCache cache,
                            IDynamicOrderFactory orderFactory,
                            IDynamicOrderRepository orderRepository,
                            IDynamicOrderTerminator terminator,
                            ITickerSubscriber tickerSubscriber)
        {
            _balanceChecker = balanceChecker;
            _cache = cache;
            _orderFactory = orderFactory;
            _orderRepository = orderRepository;
            _terminator = terminator;
            _tickerSubscriber = tickerSubscriber;
        }

        public async Task<DynamicOrder[]?> CancelTradesAsync(CancelDynamicTradeRequest request)
        {
            var canceledOrders = new List<DynamicOrder>();

            foreach (var ticker in request.Tickers)
            {
                var orders = _cache.GetGroup(ticker);

                if (orders is null || !orders.Any())
                {
                    return orders;
                }

                orders = await _terminator.ForAsync(request.Exchange, orders);

                canceledOrders.AddRange(orders);
            }

            return canceledOrders.Any() ? canceledOrders.ToArray() : null;
        }

        public async Task<DynamicOrder[]?> EditTradesAsync(EditDynamicTradeRequest request)
        {
            var orders = await _orderRepository.GetRecentOrdersByTickerAsync(request.Exchange, request.Ticker);

            if (orders is null)
            {
                return null;
            }

            var isReducingRisk = orders[0].PositionSide is PositionSide.Long ?
                !orders.Any(o => o.StopPrice > request.StopPrice) :
                !orders.Any(o => o.StopPrice < request.StopPrice);

            if (isReducingRisk is false)
            {
                return null;
            }

            foreach (var order in orders)
            {
                order.StopPrice = request.StopPrice;
            }

            await _orderRepository.UpdateOrdersAsync(orders);

            var state = _cache.GetGroupOrDefault(request.Ticker);
            state = orders;

            return orders;
        }

        public async Task<DynamicOrder[]> ExecuteTradesAsync(DynamicTradeRequest request)
        {
            var orders = request.IsWeighted ? await _orderFactory.CreateWeightedOrdersAsync(request) :
                                              await _orderFactory.CreateEqualOrdersAsync(request);

            if (!await _balanceChecker.HasEnoughInAccountAsync(request.Ticker.ToTickerRight(), request.Exchange, request.IsWeighted, orders))
            {
                throw new Exception("Not enough avalible balance on exchange.");
            }

            orders = _orderRepository.SaveNewOrders(orders);

            _tickerSubscriber.SubscribeAsync(request.Exchange, request.Ticker, TradeType.Dynamic).Wait();

            _cache.AddGroup(orders);

            return orders;
        }

        public async Task<IEnumerable<Object>?> GetTradesAsync(Exchange exchange, bool isDetailedTrades, string? ticker)
        {
            var orders = string.IsNullOrWhiteSpace(ticker) ?
                    await _orderRepository.GetActiveGroupedOrdersAsync(exchange) :
                    await _orderRepository.GetRecentOrdersByTickerAsync(exchange, ticker);

            if (orders is null || isDetailedTrades)
            {
                return orders;
            }

            var trades = new List<MiniDynamicOrder>();

            foreach (var order in orders)
            {
                trades.Add(new MiniDynamicOrder()
                {
                    BuyPrice = FormatDecimal(order.BuyPrice),
                    Fee = FormatDecimal(order.BuyFee + order.SellFee + order.FundingFee),
                    PNL = FormatDecimal(order.PNL),
                    PositionSide = order.PositionSide,
                    QuantityFilled = FormatDecimal(order.QuantityFilled),
                    QuantityQuoted = FormatDecimal(order.QuantityQuoted),
                    SellPrice = FormatDecimal(order.SellPrice),
                    Status = order.Status,
                    StopPrice = FormatDecimal(order.StopPrice),
                    TargetPrice = FormatDecimal(order.TargetPrice),
                    Ticker = order.Ticker
                });
            }

            return trades;
        }

        private decimal FormatDecimal(decimal num) =>
            num < 1 ? Math.Round(num, 8) : Math.Round(num, 3);
    }
}
