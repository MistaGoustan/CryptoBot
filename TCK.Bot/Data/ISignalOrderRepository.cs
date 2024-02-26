namespace TCK.Bot.Data
{
    public interface ISignalOrderRepository
    {
        SignalOrder CreateOrder(SignalOrder order, OrderSide side);
        Task<SignalOrder?> GetInProgressOrderByIntervalAsync(Exchange exchange, string interval, string ticker);
        Task<IEnumerable<SignalOrder>> GetOrdersByIntervalAsync(string ticker, string interval);
        Task<IEnumerable<SignalOrder>?> GetInProgressOrders();
        Task<SignalOrder?> GetOrderByOrderIdAsync(Exchange exchange, string orderId);
        Task<IEnumerable<SignalOrder>?> GetRecentOrdersAsync(Exchange exchange, short numberOfOrders, string ticker);
        SignalOrder UpdateOrder(SignalOrder order, OrderSide side);
    }
}
