using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    public sealed class DynamicPNLCalculator : IDynamicPNLCalculator
    {
        private readonly IMarketConnection _market;

        public DynamicPNLCalculator(IMarketConnection market)
        {
            _market = market;
        }

        public async Task ForOrdersAsync(DynamicOrder[] orders)
        {
            var completedOrders = orders.Where(o => o.Status is DynamicOrderStatus.Completed).ToArray();

            if (completedOrders is null || !completedOrders.Any())
            {
                return;
            }

            var fundingFee = await GetTotalFundingFeeAsync(completedOrders);

            foreach (var order in completedOrders)
            {
                order.FundingFee = fundingFee / completedOrders.Length;

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

        private async Task<Decimal> GetTotalFundingFeeAsync(DynamicOrder[] orders)
        {
            var firstBuyDate = orders[orders.Length - 1].BuyDate;
            var fundingFee = await _market.GetTotalFundingFeeAsync(orders[0].Exchange, firstBuyDate, orders[0].Ticker);

            return fundingFee;
        }
    }
}