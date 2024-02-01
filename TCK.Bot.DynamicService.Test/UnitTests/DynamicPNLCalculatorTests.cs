using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;
using TCK.Bot.DynamicService;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Bot.DynamicService.Test.UnitTests
{
    public class DynamicPNLCalculatorTests
    {
        [Fact]
        public async Task ForOrders_IsLongAndSellPriceIsAboveBuyPrice_ShouldReturnCorrectPositivePNL()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Long,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 200,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Long,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 200,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Long,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 200,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Pending
                }
            };

            var calculator = CreateCalculator(1);

            // ACT
            await calculator.ForOrdersAsync(orders);

            // ASSERT
            orders[0].PNL.ShouldBe(97.5m);
            orders[1].PNL.ShouldBe(97.5m);
            orders[2].PNL.ShouldBe(0);

            orders[0].FundingFee.ShouldBe(0.5m);
            orders[1].FundingFee.ShouldBe(0.5m);
            orders[2].FundingFee.ShouldBe(0);
        }

        [Fact]
        public async Task ForOrders_IsLongAndSellPriceIsBelowBuyPrice_ShouldReturnCorrectNegitivePNL()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Long,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 0,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Long,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 0,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Long,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 0,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Pending
                }
            };

            var calculator = CreateCalculator(-1);

            // ACT
            await calculator.ForOrdersAsync(orders);

            // ASSERT
            orders[0].PNL.ShouldBe(-101.5m);
            orders[1].PNL.ShouldBe(-101.5m);
            orders[2].PNL.ShouldBe(0);

            orders[0].FundingFee.ShouldBe(-0.5m);
            orders[1].FundingFee.ShouldBe(-0.5m);
            orders[2].FundingFee.ShouldBe(0);
        }

        [Fact]
        public async Task ForOrders_IsShortAndSellPriceIsBelowBuyPrice_ShouldReturnCorrecPositivetPNL()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Short,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 0,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Short,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 0,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Short,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 0,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Pending
                }
            };

            var calculator = CreateCalculator(1);

            // ACT
            await calculator.ForOrdersAsync(orders);

            // ASSERT
            orders[0].PNL.ShouldBe(97.5m);
            orders[1].PNL.ShouldBe(97.5m);
            orders[2].PNL.ShouldBe(0);

            orders[0].FundingFee.ShouldBe(0.5m);
            orders[1].FundingFee.ShouldBe(0.5m);
            orders[2].FundingFee.ShouldBe(0);
        }

        [Fact]
        public async Task ForOrders_IsShortAndSellPriceIsAboveBuyPrice_ShouldReturnCorrectNegitivePNL()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Short,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 200,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Short,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 200,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Completed
                },
                new DynamicOrder
                {
                    BuyPrice = 100,
                    BuyFee = 1.25m,
                    PositionSide = PositionSide.Short,
                    QuantityFilled = 1,
                    SellFee = 0.75m,
                    SellPrice = 200,
                    TargetQuantity = 1,
                    Status = DynamicOrderStatus.Pending
                }
            };

            var calculator = CreateCalculator(-1);

            // ACT
            await calculator.ForOrdersAsync(orders);

            // ASSERT
            orders[0].PNL.ShouldBe(-101.5m);
            orders[1].PNL.ShouldBe(-101.5m);
            orders[2].PNL.ShouldBe(0);

            orders[0].FundingFee.ShouldBe(-0.5m);
            orders[1].FundingFee.ShouldBe(-0.5m);
            orders[2].FundingFee.ShouldBe(0);
        }

        private DynamicPNLCalculator CreateCalculator(Decimal fundingFee)
        {
            var market = new Mock<IMarketConnection>();

            market.Setup(m => m.GetTotalFundingFeeAsync(It.IsAny<Exchange>(), It.IsAny<DateTime>(), It.IsAny<String>()).Result)
                  .Returns(fundingFee);

            return new DynamicPNLCalculator(market.Object);
        }
    }
}
