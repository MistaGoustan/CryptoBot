
namespace TCK.Bot.Services
{
    public interface IDynamicOrderUpdater
    {
        Task<DynamicOrder> UpdateOrderAsync(DynamicOrder order);
        Task<DynamicOrder[]> UpdateOrdersAsync(DynamicOrder[] orders);
    }
}