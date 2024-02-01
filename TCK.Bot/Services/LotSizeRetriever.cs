namespace TCK.Bot.Services
{
    public class LotSizeRetriever : ILotSizeRetriever
    {
        private readonly IMarketConnection _market;

        public LotSizeRetriever(IMarketConnection market)
        {
            _market = market;
        }

        public async Task<SymbolLotSizeFilter> ForSymbolAsync(Exchange exchange, String ticker)
        {
            return await _market.GetLotSizeAsync(exchange, ticker);
        }
    }
}
