namespace TCK.Bot.DynamicService
{
    public sealed class MockDynamicPNLCalculator : IDynamicPNLCalculator
    {
        public async Task ForOrdersAsync(DynamicOrder[] orders)
        {
            var completedOrders = orders.Where(o => o.Status is DynamicOrderStatus.Completed).ToArray();

            if (completedOrders is null || !completedOrders.Any())
            {
                return;
            }

            foreach (var order in completedOrders)
            {
                SetTotalPNL(order);
            }
        }

        private void SetTotalPNL(DynamicOrder order)
        {
            var sizeBought = order.BuyPrice * order.QuantityFilled;
            var sizeSold = order.SellPrice * order.TargetQuantity;
            var totaledFee = order.BuyFee + order.SellFee + order.FundingFee;

            order.PNL =
                order.PositionSide is PositionSide.Long ?
                    sizeSold - sizeBought - totaledFee :
                    sizeBought - sizeSold - totaledFee;
        }
    }
}