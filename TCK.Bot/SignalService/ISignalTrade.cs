using TCK.Bot.Api;

namespace TCK.Bot.SignalService
{
    public interface ISignalTrade
    {
        Task<IEnumerable<Object>?> GetRecentOrdersAsync(Exchange exchange, bool isDetailedTrades, short numberOfOrders, string ticker);
        Task<SignalOrder> TradeAsync(SignalTradeRequest request);
    }
}