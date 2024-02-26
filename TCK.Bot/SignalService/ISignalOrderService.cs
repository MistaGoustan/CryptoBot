namespace TCK.Bot.SignalService
{
    public interface ISignalOrderService
    {
        Task<PlacedOrder> PlaceLimitBuyAsync(Exchange exchange, decimal price, decimal quantity, string ticker);
        Task<PlacedOrder> PlaceLimitSellAsync(Exchange exchange, decimal price, decimal quantity, string ticker);
        Task<PlacedOrder> PlaceMarketBuyAsync(Exchange exchange, decimal price, decimal quantity, PositionSide side, string ticker);
        Task<PlacedOrder> PlaceMarketSellAsync(Exchange exchange, decimal price, decimal quantity, PositionSide side, string ticker);
    }
}