namespace TCK.Bot.Services
{
    public sealed class TickerValidator : ITickerValidator
    {
        private readonly IMarketConnection _market;

        public TickerValidator(IMarketConnection market)
        {
            _market = market;
        }

        public async Task<Boolean> TickerPairExistsAsync(Exchange exchange, String ticker)
        {
            return await _market.TickerPairExistsAsync(exchange, ticker);
        }
    }
}
