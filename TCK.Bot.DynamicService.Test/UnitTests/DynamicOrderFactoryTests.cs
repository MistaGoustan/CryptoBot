using Moq;
using Shouldly;
using TCK.Bot.Api;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Bot.DynamicService.Test.UnitTests
{
    public class DynamicOrderFactoryTests
    {
        private readonly DynamicOrderFactory _subject;

        public DynamicOrderFactoryTests()
        {
            var retriever = new Mock<ILotSizeRetriever>();
            retriever
                .Setup(r => r.ForSymbolAsync(It.IsAny<Exchange>(), It.IsAny<String>()).Result)
                .Returns(new SymbolLotSizeFilter { MaxQuantity = 500, MinQuantity = 1, StepSize = 1 });

            var sizer = new Mock<IDynamicPositionSizer>();
            sizer
                .Setup(s => s.GetDynamicBuySizeAsync(It.IsAny<Decimal>(), It.IsAny<Decimal>(), It.IsAny<Exchange>(), It.IsAny<Decimal>(), It.IsAny<String>()).Result)
                .Returns(1000);

            _subject = new DynamicOrderFactory(retriever.Object, sizer.Object);
        }

        [Fact]
        public async Task CreateOrdersAsync_ShouldCreateSpecifiedAmountOfOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                Ticker = "ETHUSDT",
                NumberOfOrders = 5
            };

            // ACT
            var result = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            result.Length.ShouldBe(5);
        }

        [Fact]
        public async Task CreateOrdersAsync_ShouldInitializeOrderValuesProperly()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                Ticker = "ETHUSDT",
                NumberOfOrders = 3,
                PositionSide = PositionSide.Short,
                StopPrice = 69
            };

            // ACT
            var orders = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            foreach (var order in orders)
            {
                order.Status.ShouldBe(DynamicOrderStatus.Pending);
                order.PositionSide.ShouldBe(PositionSide.Short);
                order.StopPrice.ShouldBe(69);
            }
            orders.Length.ShouldBe(3);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenLong_ShouldSetBuyPricesWithinSpecifiedRangeForEqualOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = false,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperBuyPrice = 100,
                LowerBuyPrice = 0,
                PositionSide = PositionSide.Long
            };

            // ACT
            var orders = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            orders[0].BuyPrice.ShouldBe(100);
            orders[1].BuyPrice.ShouldBe(75);
            orders[2].BuyPrice.ShouldBe(50);
            orders[3].BuyPrice.ShouldBe(25);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenShort_ShouldSetBuyPricesWithinSpecifiedRangeForEqualOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = false,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperBuyPrice = 100,
                LowerBuyPrice = 0,
                PositionSide = PositionSide.Short
            };

            // ACT
            var orders = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            orders[0].BuyPrice.ShouldBe(75);
            orders[1].BuyPrice.ShouldBe(50);
            orders[2].BuyPrice.ShouldBe(25);
            orders[3].BuyPrice.ShouldBe(0);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenLong_ShouldSetBuyPricesWithinSpecifiedRangeForWeightedOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = true,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperBuyPrice = 100,
                LowerBuyPrice = 0,
                PositionSide = PositionSide.Long
            };

            // ACT
            var orders = await _subject.CreateWeightedOrdersAsync(request);

            // ASSERT
            orders[0].BuyPrice.ShouldBe(100);
            orders[1].BuyPrice.ShouldBe(75);
            orders[2].BuyPrice.ShouldBe(50);
            orders[3].BuyPrice.ShouldBe(25);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenShort_ShouldSetBuyPricesWithinSpecifiedRangeForWeightedOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = true,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperBuyPrice = 100,
                LowerBuyPrice = 0,
                PositionSide = PositionSide.Short
            };

            // ACT
            var orders = await _subject.CreateWeightedOrdersAsync(request);

            // ASSERT
            orders[0].BuyPrice.ShouldBe(75);
            orders[1].BuyPrice.ShouldBe(50);
            orders[2].BuyPrice.ShouldBe(25);
            orders[3].BuyPrice.ShouldBe(0);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenLong_ShouldSetTargetPricesWithinSpecifiedRangeForEqualOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = false,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperTargetPrice = 200,
                LowerTargetPrice = 100,
                PositionSide = PositionSide.Long
            };

            // ACT
            var orders = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            orders[0].TargetPrice.ShouldBe(100);
            orders[1].TargetPrice.ShouldBe(125);
            orders[2].TargetPrice.ShouldBe(150);
            orders[3].TargetPrice.ShouldBe(175);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenShort_ShouldSetTargetPricesWithinSpecifiedRangeForEqualOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = false,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperTargetPrice = 200,
                LowerTargetPrice = 100,
                PositionSide = PositionSide.Short
            };

            // ACT
            var orders = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            orders[0].TargetPrice.ShouldBe(125);
            orders[1].TargetPrice.ShouldBe(150);
            orders[2].TargetPrice.ShouldBe(175);
            orders[3].TargetPrice.ShouldBe(200);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenLong_ShouldSetTargetPricesWithinSpecifiedRangeForWeightedOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = true,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperTargetPrice = 200,
                LowerTargetPrice = 100,
                PositionSide = PositionSide.Long
            };

            // ACT
            var orders = await _subject.CreateWeightedOrdersAsync(request);

            // ASSERT
            orders[0].TargetPrice.ShouldBe(100);
            orders[1].TargetPrice.ShouldBe(125);
            orders[2].TargetPrice.ShouldBe(150);
            orders[3].TargetPrice.ShouldBe(175);
        }

        [Fact]
        public async Task CreateOrdersAsync_WhenShort_ShouldSetTargetPricesWithinSpecifiedRangeForWeightedOrders()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = true,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4,
                UpperTargetPrice = 200,
                LowerTargetPrice = 100,
                PositionSide = PositionSide.Short
            };

            // ACT
            var orders = await _subject.CreateWeightedOrdersAsync(request);

            // ASSERT
            orders[0].TargetPrice.ShouldBe(125);
            orders[1].TargetPrice.ShouldBe(150);
            orders[2].TargetPrice.ShouldBe(175);
            orders[3].TargetPrice.ShouldBe(200);
        }

        [Fact]
        public async Task CreateOrdersAsync_ShouldSetEqualQuantities()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = false,
                Ticker = "ETHUSDT",
                NumberOfOrders = 5
            };

            // ACT
            var orders = await _subject.CreateEqualOrdersAsync(request);

            // ASSERT
            foreach (var order in orders)
            {
                order.QuantityQuoted.ShouldBe(200);
            }
        }

        [Fact]
        public async Task CreateOrdersAsync_ShouldSetWeightedQuantities()
        {
            // ARRANGE
            var request = new DynamicTradeRequest
            {
                IsWeighted = true,
                Ticker = "ETHUSDT",
                NumberOfOrders = 4
            };

            // ACT
            var orders = await _subject.CreateWeightedOrdersAsync(request);

            // ASSERT
            orders[0].QuantityQuoted.ShouldBe(100);
            orders[1].QuantityQuoted.ShouldBe(200);
            orders[2].QuantityQuoted.ShouldBe(300);
            orders[3].QuantityQuoted.ShouldBe(400);
        }
    }
}
