using TCK.Bot.Api;

namespace TCK.Bot.DynamicService
{
    public interface IDynamicTrade
    {
        Task<DynamicOrder[]?> CancelTradesAsync(CancelDynamicTradeRequest request);
        Task<DynamicOrder[]?> EditTradesAsync(EditDynamicTradeRequest request);
        Task<DynamicOrder[]> ExecuteTradesAsync(DynamicTradeRequest request);
        Task<IEnumerable<Object>?> GetTradesAsync(Exchange exchange, Boolean isDetailedTrades, String? ticker);
    }
}