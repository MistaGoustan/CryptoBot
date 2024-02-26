namespace TCK.Bot.SignalService
{
    public sealed class SignalPNLCalculator : ISignalPNLCalculator
    {
        public decimal ForOrder(SignalOrder order)
        {
            var sizeBought = order.BuyPrice * order.Quantity;
            var sizeSold = order.SellPrice * order.Quantity;
            var totaledFee = order.BuyFee + order.SellFee + order.FundingFee;

            var pnl = order.PositionSide is PositionSide.Long ?
                sizeSold - sizeBought - totaledFee :
                sizeBought - sizeSold - totaledFee;

            return pnl;
        }
    }
}
