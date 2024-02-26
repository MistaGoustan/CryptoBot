namespace TCK.Bot.Services
{
    public sealed class TickerValidator : ITickerValidator
    {
        private readonly IMarketConnection _market;

        public TickerValidator(IMarketConnection market)
        {
            _market = market;
        }

        public async Task<bool> TickerPairExistsAsync(Exchange exchange, string ticker)
        {
            return await _market.TickerPairExistsAsync(exchange, ticker);
        }
    }
}
