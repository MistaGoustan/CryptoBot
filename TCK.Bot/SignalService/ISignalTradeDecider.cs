namespace TCK.Bot.SignalService
{
    public interface ISignalTradeDecider
    {
        Task<Boolean> CanTradeAsync(String interval, OrderSide orderSide, String ticker);
    }
}
