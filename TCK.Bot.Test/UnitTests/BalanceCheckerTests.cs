using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCK.Bot.Binance;
using TCK.Bot.DynamicService;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Bot.Test.UnitTests
{
    public class BalanceCheckerTests
    {
        [Theory]
        // Equal Qty
        [InlineData(-1, false, false)]
        [InlineData(0, false, false)]
        [InlineData(1, false, false)]
        [InlineData(400, true, false)]
        [InlineData(401, true, false)]
        // Weighted Qty
        [InlineData(-1, false, true)]
        [InlineData(0, false, true)]
        [InlineData(1, false, true)]
        [InlineData(400, true, true)]
        [InlineData(401, true, true)]
        public async Task HasEnoughInAccountAsync_ShouldCheckBalanceAccordingly(Decimal availableBalance, Boolean expectedValue, Boolean isWeighted)
        {
            // ARRANGE
            var uncachedOrders = CreateUncachedOrders();
            var cachedOrderGroups = CreateCachedOrderGroups();

            var binance = new Mock<IBinanceSpotMarketConnection>();
            binance.Setup(m => m.GetAvailableBalanceAsync(It.IsAny<String>()))
                  .Returns(Task.FromResult(availableBalance));

            var cache = new Mock<IDynamicOrderCache>();
            cache.Setup(c => c.GetAllGroups()).Returns(cachedOrderGroups);

            var subject = new BalanceChecker(binance.Object, cache.Object);

            // ACT
            var result = await subject.HasEnoughInAccountAsync(It.IsAny<String>(), Exchange.Binance, isWeighted, uncachedOrders);

            // ASSERT
            result.ShouldBe(expectedValue);
        }

        private List<DynamicOrder[]> CreateCachedOrderGroups() =>
            new List<DynamicOrder[]>
            {
                new DynamicOrder[]
                {
                        new DynamicOrder
                        {
                            BuyPrice = 100,
                            Exchange = Exchange.Binance,
                            Status = DynamicOrderStatus.Pending,
                            QuantityQuoted = 1
                        },
                    new DynamicOrder
                    {
                        BuyPrice = 100,
                        Exchange = Exchange.Binance,
                        Status = DynamicOrderStatus.Pending,
                        QuantityQuoted = 1
                    },
                    new DynamicOrder
                    {
                        BuyPrice = 100,
                        Exchange = Exchange.Binance,
                        Status = DynamicOrderStatus.Completed,
                        QuantityQuoted = 1
                    }
                },
                new DynamicOrder[]
                {
                    new DynamicOrder
                    {
                        BuyPrice = 100,
                        Exchange = Exchange.Binance,
                        Status = DynamicOrderStatus.Pending,
                        QuantityQuoted = 1
                    }
                }
            };

        private DynamicOrder[] CreateUncachedOrders() =>
            new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    Status = DynamicOrderStatus.Pending,
                    QuantityQuoted = 1
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    Status = DynamicOrderStatus.Pending,
                    QuantityQuoted = 1
                },
            };
    }
}
