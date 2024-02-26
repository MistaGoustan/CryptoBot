
namespace TCK.Bot.Data
{
    public interface IDynamicOrderRepository
    {
        Task<DynamicOrder[]?> GetActiveGroupedOrdersAsync(Exchange exchange);
        Task<IEnumerable<DynamicOrder>?> GetOrdersByOrderGroupIdAsync(string orderGroupId);
        Task<DynamicOrder[]?> GetRecentOrdersByTickerAsync(Exchange exchange, string ticker);
        Task<DynamicOrder[]> GetUncompletedOrdersAsync();
        DynamicOrder[] SaveNewOrders(DynamicOrder[] orders);
        Task<DynamicOrder> UpdateOrderAsync(DynamicOrder order);
        Task<DynamicOrder[]> UpdateOrdersAsync(DynamicOrder[] orders);
        Task<DynamicOrder[]> UpdateOrdersAsync(DynamicOrder[] orders, OrderSide side);
    }
}