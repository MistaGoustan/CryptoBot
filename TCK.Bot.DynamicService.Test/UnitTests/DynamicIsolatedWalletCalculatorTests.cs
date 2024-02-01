using Moq;
using Shouldly;
using TCK.Bot.Data;
using Xunit;

namespace TCK.Bot.DynamicService.Test.UnitTests
{
    public class DynamicIsolatedWalletCalculatorTests
    {
        [Theory]
        [InlineData(100, 3, 3, 697, 1000)]
        [InlineData(100.5, 3, 3, 695.5, 1000)]
        [InlineData(100, 1.5, 3, 698.5, 1000)]
        [InlineData(100, 3, 1.5, 847, 1000)]
        public async Task ForBuyOrder_ShouldUpdateWalletCorrectly(Decimal buyPrice, Decimal fee, Decimal quantity, Decimal expectedAvailableBalance, Decimal expectedBalance)
        {
            //ARRANGE
            var order = new DynamicOrder { Exchange = Exchange.Binance, BuyPrice = buyPrice, BuyFee = fee, QuantityQuoted = quantity, Ticker = "ETHUSDT" };

            var dynamicWalletRepo = new Mock<IDynamicIsolatedWalletRepository>();
            dynamicWalletRepo
                .Setup(d => d.GetWalletAsync(Exchange.Binance, It.IsAny<String>()).Result)
                .Returns(new DynamicIsolatedWallet { AvailableBalance = 1000, Balance = 1000, Ticker = "ETHUSDT" });

            var subject = new DynamicIsolatedWalletProcessor(dynamicWalletRepo.Object);

            //ACT
            var result = await subject.ForBuyOrderAsync(order);

            // ASSERT
            result.AvailableBalance.ShouldBe(expectedAvailableBalance);
            result.Balance.ShouldBe(expectedBalance);
        }

        [Theory]
        [InlineData(100, 3, 3, 797, 797)]
        [InlineData(100.5, 3, 3, 798.5, 798.5)]
        [InlineData(100, 1.5, 3, 798.5, 798.5)]
        [InlineData(100, 3, 1.5, 647, 647)]
        public async Task ForSellOrder_ShouldUpdateWalletCorrectly(Decimal sellPrice, Decimal fee, Decimal quantity, Decimal expectedAvailableBalance, Decimal expectedBalance)
        {
            //ARRANGE
            var order = new DynamicOrder { Exchange = Exchange.Binance, SellPrice = sellPrice, SellFee = fee, QuantityQuoted = quantity, Ticker = "ETHUSDT" };

            var dynamicWalletRepo = new Mock<IDynamicIsolatedWalletRepository>();
            dynamicWalletRepo
                .Setup(d => d.GetWalletAsync(Exchange.Binance, It.IsAny<String>()).Result)
                .Returns(new DynamicIsolatedWallet { AvailableBalance = 500, Balance = 1000, Ticker = "ETHUSDT" });

            var subject = new DynamicIsolatedWalletProcessor(dynamicWalletRepo.Object);

            //ACT
            var result = await subject.ForSellOrderAsync(order);

            // ASSERT
            result.AvailableBalance.ShouldBe(expectedAvailableBalance);
            result.Balance.ShouldBe(expectedBalance);
        }
    }
}
