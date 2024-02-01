namespace TCK.Bot.Binance
{
    public interface IBinanceOrderService
    {
        Task<PlacedOrder> PlaceMarketBuyAsync(Decimal price, Decimal quantity, String ticker);
        Task<PlacedOrder> PlaceMarketSellAsync(Decimal price, Decimal quantity, String ticker);
        Task<PlacedOrder> PlaceLimitBuyAsync(String ticker, Decimal quantity, Decimal price);
        Task<PlacedOrder> PlaceLimitSellAsync(String ticker, Decimal quantity, Decimal price);
    }
}
