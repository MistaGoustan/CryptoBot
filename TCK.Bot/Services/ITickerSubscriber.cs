namespace TCK.Bot.Services
{
    public interface ITickerSubscriber
    {
        Task SubscribeAsync(Exchange exchange, String ticker, TradeType tradeType);
    }
}