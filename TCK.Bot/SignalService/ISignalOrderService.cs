namespace TCK.Bot.SignalService
{
    public interface ISignalOrderService
    {
        Task<PlacedOrder> PlaceLimitBuyAsync(Exchange exchange, Decimal price, Decimal quantity, String ticker);
        Task<PlacedOrder> PlaceLimitSellAsync(Exchange exchange, Decimal price, Decimal quantity, String ticker);
        Task<PlacedOrder> PlaceMarketBuyAsync(Exchange exchange, Decimal price, Decimal quantity, PositionSide side, String ticker);
        Task<PlacedOrder> PlaceMarketSellAsync(Exchange exchange, Decimal price, Decimal quantity, PositionSide side, String ticker);
    }
}