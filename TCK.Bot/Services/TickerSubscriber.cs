using TCK.Bot.Binance;

namespace TCK.Bot.Services
{
    public class TickerSubscriber : ITickerSubscriber
    {
        private readonly IBinanceTickerSubscriber _binance;
        public TickerSubscriber(IBinanceTickerSubscriber binance)
        {
            _binance = binance;
        }

        public async Task SubscribeAsync(Exchange exchange, String ticker, TradeType tradeType)
        {
            switch (exchange)
            {
                case Exchange.Binance:
                    await _binance.SubscribeAsync(ticker);
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"No option to subscribe to {ticker} on {exchange}.");
            }
        }
    }
}
