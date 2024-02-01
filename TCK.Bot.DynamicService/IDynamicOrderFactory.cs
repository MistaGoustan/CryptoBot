using TCK.Bot.Api;

namespace TCK.Bot.DynamicService
{
    public interface IDynamicOrderFactory
    {
        Task<DynamicOrder[]> CreateEqualOrdersAsync(DynamicTradeRequest request);
        Task<DynamicOrder[]> CreateWeightedOrdersAsync(DynamicTradeRequest request);
    }
}