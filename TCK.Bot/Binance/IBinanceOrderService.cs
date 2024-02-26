namespace TCK.Bot.Binance
{
    public interface IBinanceOrderService
    {
        Task<PlacedOrder> PlaceMarketBuyAsync(decimal price, decimal quantity, string ticker);
        Task<PlacedOrder> PlaceMarketSellAsync(decimal price, decimal quantity, string ticker);
        Task<PlacedOrder> PlaceLimitBuyAsync(string ticker, decimal quantity, decimal price);
        Task<PlacedOrder> PlaceLimitSellAsync(string ticker, decimal quantity, decimal price);
    }
}
