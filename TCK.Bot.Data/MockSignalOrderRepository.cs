using Microsoft.Extensions.Options;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.Data
{
    internal sealed class MockSignalOrderRepository : ISignalOrderRepository
    {
        private readonly ISignalOrderRepository _innerRepo;

        public MockSignalOrderRepository(IOptions<ConfigurationOptions> options,
                                         ISignalOrderRepository innerRepo,
                                         IIsolatedWalletService wallet)
        {
            _innerRepo = innerRepo;
        }

        public SignalOrder CreateOrder(SignalOrder order, OrderSide side)
        {
            return _innerRepo.CreateOrder(order, side);
        }

        public Task<SignalOrder?> GetInProgressOrderByIntervalAsync(Exchange exchange, String ticker, String interval) =>
            _innerRepo.GetInProgressOrderByIntervalAsync(exchange, ticker, interval);

        public Task<IEnumerable<SignalOrder>?> GetInProgressOrders() =>
            _innerRepo.GetInProgressOrders();

        public Task<IEnumerable<SignalOrder>> GetOrdersByIntervalAsync(String ticker, String interval) =>
            _innerRepo.GetOrdersByIntervalAsync(ticker, interval);

        public Task<SignalOrder?> GetOrderByOrderIdAsync(Exchange exchange, String orderId) =>
            _innerRepo.GetOrderByOrderIdAsync(exchange, orderId);

        public Task<IEnumerable<SignalOrder>?> GetRecentOrdersAsync(Exchange exchange, Int16 numberOfOrders, String ticker) =>
            _innerRepo.GetRecentOrdersAsync(exchange, numberOfOrders, ticker);

        public SignalOrder UpdateOrder(SignalOrder order, OrderSide side)
        {
            return _innerRepo.UpdateOrder(order, side);
        }
    }
}
