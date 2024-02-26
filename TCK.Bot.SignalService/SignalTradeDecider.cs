using TCK.Bot.Data;

namespace TCK.Bot.SignalService
{
    public sealed class SignalTradeDecider : ISignalTradeDecider
    {
        private readonly ISignalOrderRepository _orderRepository;
        public SignalTradeDecider(ISignalOrderRepository orderRepository) =>
            _orderRepository = orderRepository;

        public async Task<bool> CanTradeAsync(string interval, OrderSide orderSide, string ticker)
        {
            var orders = await _orderRepository.GetOrdersByIntervalAsync(ticker, interval);

            return orderSide switch
            {
                OrderSide.Buy => CanBuy(orders),
                OrderSide.Sell => CanSell(orders),
                _ => throw new Exception("Cannot trade with given OrderSide."),
            };
        }

        private bool CanBuy(IEnumerable<SignalOrder> orders) =>
            !orders.Any(o => o.Status == SignalOrderStatus.InProgress);

        private bool CanSell(IEnumerable<SignalOrder> orders) =>
            orders.Any(o => o.Status == SignalOrderStatus.InProgress);
    }
}
