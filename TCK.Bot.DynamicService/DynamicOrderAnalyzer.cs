using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.DynamicService
{
    public class DynamicOrderAnalyzer : IDynamicOrderAnalyzer
    {
        private readonly decimal _moveUpStopPricePercent;
        private readonly IInProgressOrderAnalyzer _inProgressOrderAnalyzer;
        private readonly ILogger<DynamicOrderAnalyzer> _logger;
        private readonly IDynamicOrderUpdater _orderUpdater;
        private readonly IPendingOrderAnalyzer _pendingOrderAnalyzer;
        private readonly IDynamicPNLCalculator _pnl;

        public DynamicOrderAnalyzer(IOptions<ConfigurationOptions> configuration,
                                    IInProgressOrderAnalyzer inProgressOrderAnalyzer,
                                    ILogger<DynamicOrderAnalyzer> logger,
                                    IDynamicOrderUpdater orderUpdater,
                                    IPendingOrderAnalyzer pendingOrderAnalyzer,
                                    IDynamicPNLCalculator pnl)
        {
            _moveUpStopPricePercent = configuration.Value.DynamicMoveUpStopPricePercent;
            _inProgressOrderAnalyzer = inProgressOrderAnalyzer;
            _logger = logger;
            _orderUpdater = orderUpdater;
            _pendingOrderAnalyzer = pendingOrderAnalyzer;
            _pnl = pnl;
        }

        public async Task ForPriceAsync(decimal lastPrice, DynamicOrder[] orders)
        {
            var hasPendingUpdate = false;
            var hasInProgressUpdate = false;

            foreach (var order in orders)
            {
                if (order.Status is DynamicOrderStatus.Pending)
                {
                    var result = await _pendingOrderAnalyzer.ForPriceWithTickerAsync(lastPrice, order);

                    hasPendingUpdate = hasPendingUpdate ? hasPendingUpdate : result;
                }
                else if (order.Status is DynamicOrderStatus.InProgress)
                {
                    var result = await _inProgressOrderAnalyzer.ForPriceWithTickerAsync(lastPrice, order);

                    hasInProgressUpdate = hasInProgressUpdate ? hasInProgressUpdate : result;
                }
            }

            if (hasPendingUpdate)
            {
                _logger.LogInformation($"PendingUpdate - {orders[0].Ticker}: {lastPrice}");

                foreach (var order in orders)
                {
                    SetTargetQuantities(order, orders);
                }

                await _orderUpdater.UpdateOrdersAsync(orders);
            }
            else if (hasInProgressUpdate)
            {
                _logger.LogInformation($"InProgressUpdate - {orders[0].Ticker}: {lastPrice}");

                foreach (var order in orders)
                {
                    CancelPendingOrder(order);
                    MoveStopPrices(order, orders);
                }

                await _pnl.ForOrdersAsync(orders);
                await _orderUpdater.UpdateOrdersAsync(orders);
            }
        }

        private void CancelPendingOrder(DynamicOrder order)
        {
            if (order.Status is DynamicOrderStatus.Pending)
            {
                order.Status = DynamicOrderStatus.Canceled;

                _logger.LogInformation($"Pending Order Canceled - ID {order.Id}");
            }
        }

        private decimal GetAveragedQuantityFilled(DynamicOrder[] orders)
        {
            var inProgressOrders = orders.Where(o => o.Status is DynamicOrderStatus.InProgress);
            var totalQty = inProgressOrders.Sum(o => o.QuantityFilled);

            return totalQty / inProgressOrders.Count();
        }

        private void MoveStopPrices(DynamicOrder order, DynamicOrder[] orders)
        {
            if (order.StopPrice == order.BuyPrice)
            {
                return;
            }

            decimal amountCompleted = orders.Where(o => o.Status is DynamicOrderStatus.Completed).Count();
            var percentOfCompletedOrders = amountCompleted / orders.Length;

            if (percentOfCompletedOrders >= _moveUpStopPricePercent)
            {
                order.StopPrice = order.BuyPrice;

                _logger.LogInformation($"StopPrice Moved - Completed Orders %: {percentOfCompletedOrders} >= Config %: {_moveUpStopPricePercent} - ID {order.Id}");
            }
        }

        private void SetTargetQuantities(DynamicOrder order, DynamicOrder[] orders)
        {
            var avgQty = GetAveragedQuantityFilled(orders);

            if (order.Status is DynamicOrderStatus.InProgress)
            {
                _logger.LogInformation($"Target Qty Changed from {order.TargetQuantity} to {avgQty} - ID {order.Id}");

                order.TargetQuantity = avgQty;
            }
        }
    }
}
