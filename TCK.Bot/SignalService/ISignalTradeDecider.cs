namespace TCK.Bot.SignalService
{
    public interface ISignalTradeDecider
    {
        Task<bool> CanTradeAsync(string interval, OrderSide orderSide, string ticker);
    }
}
