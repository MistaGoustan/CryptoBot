using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using TCK.Bot.Options;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Bot.DynamicService.Test.UnitTests
{
    public class DynamicOrderAnalyzerTests
    {
        [Fact]
        public async Task ForPrice_ShouldSetStopPriceToBreakEven()
        {
            // ARRANGE
            var moveStopPricePercent = 0.5m;
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 105,
                    Status = DynamicOrderStatus.InProgress,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 110,
                    Status = DynamicOrderStatus.Completed,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 115,
                    Status = DynamicOrderStatus.Completed,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 120,
                    Status = DynamicOrderStatus.Completed,
                    StopPrice = 90
                }
            };
            var subject = CreateAnalyzer(true, false, moveStopPricePercent);

            // ACT
            await subject.ForPriceAsync(0, orders);

            // ASSERT
            foreach (var order in orders)
            {
                order.StopPrice.ShouldBe(order.BuyPrice);
            }
        }

        [Fact]
        public async Task ForPrice_ShouldCancelPendingOrder_WhenThereIsAnInProgressUpdate()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    Status = DynamicOrderStatus.Pending,
                },
                new DynamicOrder
                {
                    BuyPrice = 105,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 110,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 115,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 120,
                    Status = DynamicOrderStatus.InProgress,
                    StopPrice = 90
                }
            };
            var subject = CreateAnalyzer(true, false, 0);

            // ACT
            await subject.ForPriceAsync(0, orders);

            // ASSERT
            for (var i = 0; i < orders.Length - 1; i++) // "- 1" all orders except for the last one
            {
                orders[i].Status = DynamicOrderStatus.Canceled;
            }
        }

        [Fact]
        public async Task ForPrice_()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    Status = DynamicOrderStatus.Pending,
                },
                new DynamicOrder
                {
                    BuyPrice = 105,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 110,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 115,
                    Status = DynamicOrderStatus.Pending,
                    StopPrice = 90
                },
                new DynamicOrder
                {
                    BuyPrice = 120,
                    Status = DynamicOrderStatus.InProgress,
                    StopPrice = 90
                }
            };
            var subject = CreateAnalyzer(true, false, 0);

            // ACT
            await subject.ForPriceAsync(0, orders);

            // ASSERT
            for (var i = 0; i < orders.Length - 1; i++) // "- 1" all orders except for the last one
            {
                orders[i].Status = DynamicOrderStatus.Canceled;
            }
        }

        [Fact]
        public async Task SetTargetQuantities_AllOrdersAreInProgressAndQuantitesAreEqual_ShouldUpdateTargetQuantitesEqually()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder { BuyPrice = 100, QuantityFilled = 1, Status = DynamicOrderStatus.InProgress },
                new DynamicOrder { BuyPrice = 100, QuantityFilled = 1, Status = DynamicOrderStatus.InProgress },
                new DynamicOrder { BuyPrice = 200, QuantityFilled = 1, Status = DynamicOrderStatus.Pending }
            };

            var subject = CreateAnalyzer(false, true, 0);

            // ACT
            await subject.ForPriceAsync(0, orders);

            // ASSERT
            for (var i = 0; i < orders.Length - 1; i++) // "- 1" all orders except for the last one
            {
                orders[i].TargetQuantity.ShouldBe(1);
            }
        }

        [Fact]
        public async Task SetTargetQuantities_AllOrdersAreInProgressAndQuantitesAreWeighted_ShouldUpdateTargetQuantitesEqually()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder { BuyPrice = 100, QuantityFilled = 1, Status = DynamicOrderStatus.InProgress },
                new DynamicOrder { BuyPrice = 100, QuantityFilled = 2, Status = DynamicOrderStatus.InProgress },
                new DynamicOrder { BuyPrice = 100, QuantityFilled = 3, Status = DynamicOrderStatus.InProgress },
                new DynamicOrder { BuyPrice = 200, QuantityFilled = 4, Status = DynamicOrderStatus.Pending }
            };

            var subject = CreateAnalyzer(false, true, 0);

            // ACT
            await subject.ForPriceAsync(0, orders);

            // ASSERT
            for (var i = 0; i < orders.Length - 1; i++) // "- 1" all orders except for the last one
            {
                orders[i].TargetQuantity.ShouldBe(2);
            }
        }

        private DynamicOrderAnalyzer CreateAnalyzer(bool hasInProgressUpdate, bool hasPendingUpdate, decimal moveStopPricePercent)
        {
            var config = new Mock<IOptions<ConfigurationOptions>>();
            config.Setup(c => c.Value.DynamicMoveUpStopPricePercent).Returns(moveStopPricePercent);

            var inProgressAnalyzer = new Mock<IInProgressOrderAnalyzer>();
            inProgressAnalyzer.Setup(i => i.ForPriceWithTickerAsync(It.IsAny<decimal>(), It.IsAny<DynamicOrder>()).Result).Returns(hasInProgressUpdate);

            var orderUpdater = new Mock<IDynamicOrderUpdater>();

            var pendingAnalyzer = new Mock<IPendingOrderAnalyzer>();
            pendingAnalyzer.Setup(i => i.ForPriceWithTickerAsync(It.IsAny<decimal>(), It.IsAny<DynamicOrder>()).Result).Returns(hasPendingUpdate);

            var pnl = new Mock<IDynamicPNLCalculator>();

            return new DynamicOrderAnalyzer(config.Object, inProgressAnalyzer.Object, CreateLogger(), orderUpdater.Object, pendingAnalyzer.Object, pnl.Object);
        }

        private ILogger<DynamicOrderAnalyzer> CreateLogger()
        {
            var serviceProvider = new ServiceCollection()
                                      .AddLogging()
                                      .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            return factory!.CreateLogger<DynamicOrderAnalyzer>();
        }
    }
}
