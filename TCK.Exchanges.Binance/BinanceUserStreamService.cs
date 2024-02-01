using Microsoft.Extensions.Options;
using TCK.Bot.Binance;
using TCK.Bot.Options;

namespace TCK.Exchanges.Binance
{
    internal class BinanceUserStreamService : BinanceBase, IBinanceUserStreamService
    {
        public BinanceUserStreamService(IOptions<BinanceOptions> options)
            : base(options) { }

        public void ReconnectToUserStream()
        {
            Client.SpotApi.Account.KeepAliveUserStreamAsync(ListenKey);
        }
    }
}
