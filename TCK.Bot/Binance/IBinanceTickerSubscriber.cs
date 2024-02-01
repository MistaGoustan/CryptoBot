namespace TCK.Bot.Binance
{
    public interface IBinanceTickerSubscriber
    {
        public Task SubscribeAsync(String ticker);
    }
}