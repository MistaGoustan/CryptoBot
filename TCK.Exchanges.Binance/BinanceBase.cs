using Binance.Net.Clients;
using Binance.Net.Objects;
using Microsoft.Extensions.Options;
using TCK.Bot.Options;

namespace TCK.Exchanges.Binance
{
    public abstract class BinanceBase
    {
        private readonly string _apiKey;
        private readonly string _secret;

        public BinanceBase(IOptions<BinanceOptions> options)
        {
            _apiKey = options.Value.ApiKey;
            _secret = options.Value.Secret;
        }

        protected BinanceClient Client => new(new()
        {
            ApiCredentials = new(_apiKey, _secret),
            SpotApiOptions = new() { BaseAddress = BinanceApiAddresses.Us.RestClientAddress }
        });

        protected BinanceSocketClient SocketClient => new(new BinanceSocketClientOptions()
        {
            ApiCredentials = new(_apiKey, _secret),
            SpotApiOptions = new() { BaseAddress = BinanceApiAddresses.Us.SocketClientAddress }
        });


        private string _listenKey = string.Empty;
        protected string ListenKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_listenKey))
                {
                    _listenKey = GetListenKey().GetAwaiter().GetResult();
                }

                return _listenKey;
            }
        }

        private async Task<string> GetListenKey()
        {
            var listenKeyResult = await Client.SpotApi.Account.StartUserStreamAsync();

            if (!listenKeyResult.Success)
            {
                throw new Exception($"ListenKey Failure: {listenKeyResult.Error}");
            }

            return listenKeyResult.Data;
        }
    }
}