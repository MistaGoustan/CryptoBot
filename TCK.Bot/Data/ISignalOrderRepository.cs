namespace TCK.Bot.Data
{
    public interface ISignalOrderRepository
    {
        SignalOrder CreateOrder(SignalOrder order, OrderSide side);
        Task<SignalOrder?> GetInProgressOrderByIntervalAsync(Exchange exchange, String interval, String ticker);
        Task<IEnumerable<SignalOrder>> GetOrdersByIntervalAsync(String ticker, String interval);
        Task<IEnumerable<SignalOrder>?> GetInProgressOrders();
        Task<SignalOrder?> GetOrderByOrderIdAsync(Exchange exchange, String orderId);
        Task<IEnumerable<SignalOrder>?> GetRecentOrdersAsync(Exchange exchange, Int16 numberOfOrders, String ticker);
        SignalOrder UpdateOrder(SignalOrder order, OrderSide side);
    }
}
