using TCK.Bot.Api;

namespace TCK.Bot.SignalService
{
    public interface ISignalTrade
    {
        Task<IEnumerable<Object>?> GetRecentOrdersAsync(Exchange exchange, Boolean isDetailedTrades, Int16 numberOfOrders, String ticker);
        Task<SignalOrder> TradeAsync(SignalTradeRequest request);
    }
}