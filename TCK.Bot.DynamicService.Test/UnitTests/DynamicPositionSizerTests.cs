using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using TCK.Bot.Options;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Bot.DynamicService.Test.UnitTests
{
    public sealed class DynamicPositionSizerTests
    {
        [Theory]
        [InlineData(0, 1, 100, 10, 0.01, 0.9)]
        [InlineData(100, 1, 0, 10, 0.01, 0.9)]
        public async void DynamicBuySize_GivenSpecifiedParameters_ShouldReturnExpectedResult(Decimal accountBalance, Decimal averagePrice, Decimal exchangeBalance, Decimal expectedResult, Decimal riskPercent, Decimal stopPrice)
        {
            // ARRANGE
            var config = new Mock<IOptions<ConfigurationOptions>>();
            config.Setup(m => m.Value.IsProduction).Returns(true);
            config.Setup(m => m.Value.DynamicRiskPercent).Returns(riskPercent);

            var spotMarket = new Mock<IMarketConnection>();
            spotMarket
                .Setup(m => m.GetBalanceAsync(Exchange.Binance, It.IsAny<String>()).Result)
                .Returns(exchangeBalance);

            spotMarket
                .Setup(s => s.GetLotSizeAsync(Exchange.Binance, It.IsAny<String>()).Result)
                .Returns(new SymbolLotSizeFilter { MaxQuantity = 9000, MinQuantity = 0.00000100m, StepSize = 0.00000100m });

            var subject = new DynamicPositionSizer(config.Object, spotMarket.Object);

            // ACT
            var result = await subject.GetDynamicBuySizeAsync(accountBalance, averagePrice, Exchange.Binance, stopPrice, "ETHUSDT");

            // ASSERT
            result.ShouldBe(expectedResult);
        }
    }
}
