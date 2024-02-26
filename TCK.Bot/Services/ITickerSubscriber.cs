namespace TCK.Bot.Services
{
    public interface ITickerSubscriber
    {
        Task SubscribeAsync(Exchange exchange, string ticker, TradeType tradeType);
    }
}