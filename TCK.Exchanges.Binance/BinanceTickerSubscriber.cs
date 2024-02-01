using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TCK.Bot;
using TCK.Bot.Binance;
using TCK.Bot.DynamicService;
using TCK.Bot.Options;

namespace TCK.Exchanges.Binance
{
    public sealed class BinanceTickerSubscriber : BinanceBase, IBinanceTickerSubscriber
    {
        private readonly IDynamicOrderAnalyzer _analyzer;
        private readonly IDynamicOrderCache _cache;
        private readonly ILogger<BinanceTickerSubscriber> _logger;

        private static readonly List<Subscription> Subscriptions = new();

        public BinanceTickerSubscriber(IDynamicOrderAnalyzer analyzer,
                                       IDynamicOrderCache cache,
                                       ILogger<BinanceTickerSubscriber> logger,
                                       IOptions<BinanceOptions> options)
            : base(options)
        {
            _analyzer = analyzer;
            _cache = cache;
            _logger = logger;
        }

        public async Task SubscribeAsync(String ticker)
        {
            if (Subscriptions.Any(s => s.Ticker == ticker))
            {
                _logger.LogInformation($"Already subscribed to {ticker}");

                return;
            }

            var result = await SocketClient.SpotApi.ExchangeData.SubscribeToMiniTickerUpdatesAsync(ticker, async data =>
            {
                var orders = _cache.GetGroupOrDefault(ticker);

                if (orders != null)
                {
                    await _analyzer.ForPriceAsync(data.Data.LastPrice, orders);
                }
            });

            if (result.Success is false)
            {
                throw new Exception($"Subscription Failed: {result.Error}");
            }

            Subscriptions.Add(new Subscription { Id = result.Data.Id, Ticker = ticker });

            _logger.LogInformation($"Subscribed to {ticker}");
        }
    }
}