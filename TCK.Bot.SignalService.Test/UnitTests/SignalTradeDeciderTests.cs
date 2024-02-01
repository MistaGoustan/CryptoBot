using Moq;
using Shouldly;
using TCK.Bot.Data;
using Xunit;

namespace TCK.Bot.SignalService.Test.UnitTests
{
    public sealed class SignalTradeDeciderTests
    {
        // CAN BUY
        [Fact]
        public async void CanBuyAsyncShouldReturnFalseWhenAllOrdersAreInProgress()
        {
            // ARRANGE
            IEnumerable<SignalOrder> orders = new List<SignalOrder>()
            {
                new SignalOrder { Status = SignalOrderStatus.InProgress },
                new SignalOrder { Status = SignalOrderStatus.InProgress }
            };

            var mockOrderRepository = new Mock<ISignalOrderRepository>();
            mockOrderRepository.Setup(m => m.GetOrdersByIntervalAsync("ETHUSDT", "60")).Returns(Task.Run(() => orders));

            var decider = new SignalTradeDecider(mockOrderRepository.Object);

            // ACT
            var result = await decider.CanTradeAsync("60", OrderSide.Buy, "ETHUSDT");

            // ASSERT
            result.ShouldBeFalse();
        }

        [Fact]
        public async void CanBuyAsyncShouldReturnFalseWhenSomeOrdersAreInProgress()
        {
            // ARRANGE
            IEnumerable<SignalOrder> orders = new List<SignalOrder>()
            {
                new SignalOrder { Status = SignalOrderStatus.InProgress },
                new SignalOrder { Status = SignalOrderStatus.Completed }
            };

            var mockOrderRepository = new Mock<ISignalOrderRepository>();
            mockOrderRepository.Setup(m => m.GetOrdersByIntervalAsync("ETHUSDT", "60")).Returns(Task.Run(() => orders));

            var decider = new SignalTradeDecider(mockOrderRepository.Object);

            // ACT
            var result = await decider.CanTradeAsync("60", OrderSide.Buy, "ETHUSDT");

            // ASSERT
            result.ShouldBeFalse();
        }

        [Fact]
        public async void CanBuyAsyncShouldReturnTrueWhenNoOrdersAreInProgress()
        {
            // ARRANGE
            IEnumerable<SignalOrder> orders = new List<SignalOrder>()
            {
                new SignalOrder { Status = SignalOrderStatus.Completed },
                new SignalOrder { Status = SignalOrderStatus.Completed }
            };

            var mockOrderRepository = new Mock<ISignalOrderRepository>();
            mockOrderRepository.Setup(m => m.GetOrdersByIntervalAsync("ETHUSDT", "60")).Returns(Task.Run(() => orders));

            var decider = new SignalTradeDecider(mockOrderRepository.Object);

            // ACT
            var result = await decider.CanTradeAsync("60", OrderSide.Buy, "ETHUSDT");

            // ASSERT
            result.ShouldBeTrue();
        }

        // CAN SELL
        [Fact]
        public async void CanSellAsyncShouldReturnTrueWhenAllOrdersAreInProgress()
        {
            // ARRANGE
            IEnumerable<SignalOrder> orders = new List<SignalOrder>()
            {
                new SignalOrder { Status = SignalOrderStatus.InProgress },
                new SignalOrder { Status = SignalOrderStatus.InProgress }
            };

            var mockOrderRepository = new Mock<ISignalOrderRepository>();
            mockOrderRepository.Setup(m => m.GetOrdersByIntervalAsync("ETHUSDT", "60")).Returns(Task.Run(() => orders));

            var decider = new SignalTradeDecider(mockOrderRepository.Object);

            // ACT
            var result = await decider.CanTradeAsync("60", OrderSide.Sell, "ETHUSDT");

            // ASSERT
            result.ShouldBeTrue();
        }

        [Fact]
        public async void CanSellAsyncShouldReturnTrueWhenSomeOrdersAreInProgress()
        {
            // ARRANGE
            IEnumerable<SignalOrder> orders = new List<SignalOrder>()
            {
                new SignalOrder { Status = SignalOrderStatus.InProgress },
                new SignalOrder { Status = SignalOrderStatus.Completed }
            };

            var mockOrderRepository = new Mock<ISignalOrderRepository>();
            mockOrderRepository.Setup(m => m.GetOrdersByIntervalAsync("ETHUSDT", "60")).Returns(Task.Run(() => orders));

            var decider = new SignalTradeDecider(mockOrderRepository.Object);

            // ACT
            var result = await decider.CanTradeAsync("60", OrderSide.Sell, "ETHUSDT");

            // ASSERT
            result.ShouldBeTrue();
        }

        [Fact]
        public async void CanSellAsyncShouldReturnFalseWhenNoOrdersAreInProgress()
        {
            // ARRANGE
            IEnumerable<SignalOrder> orders = new List<SignalOrder>()
            {
                new SignalOrder { Status = SignalOrderStatus.Completed },
                new SignalOrder { Status = SignalOrderStatus.Completed }
            };

            var mockOrderRepository = new Mock<ISignalOrderRepository>();
            mockOrderRepository.Setup(m => m.GetOrdersByIntervalAsync("ETHUSDT", "60")).Returns(Task.Run(() => orders));

            var decider = new SignalTradeDecider(mockOrderRepository.Object);

            // ACT
            var result = await decider.CanTradeAsync("60", OrderSide.Sell, "ETHUSDT");

            // ASSERT
            result.ShouldBeFalse();
        }
    }
}
