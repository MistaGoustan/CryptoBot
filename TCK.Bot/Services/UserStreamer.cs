using TCK.Bot.Binance;

namespace TCK.Bot.Services
{
    public class UserStreamer : IUserStreamer
    {
        private readonly IBinanceUserStreamService _binance;

        public UserStreamer(IBinanceUserStreamService binance)
        {
            _binance = binance;
        }

        public void EstablishConnection()
        {
            _binance.ReconnectToUserStream();
        }
    }
}
