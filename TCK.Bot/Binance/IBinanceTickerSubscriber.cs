namespace TCK.Bot.Binance
{
    public interface IBinanceTickerSubscriber
    {
        public Task SubscribeAsync(string ticker);
    }
}