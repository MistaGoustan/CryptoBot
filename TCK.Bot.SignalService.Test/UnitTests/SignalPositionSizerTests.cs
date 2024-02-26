using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using TCK.Bot.Options;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Bot.SignalService.Test.UnitTests
{
    public sealed class SignalPositionSizerTests
    {
        [Theory]
        [InlineData(1000, 1, 0.01, 0.1, 100)]
        [InlineData(101, 1, 0.01, 0.1, 10.1)]
        [InlineData(100, 1, 0.01, 0.1, 10)]
        [InlineData(99, 1, 0.01, 0.1, 9.9)]
        [InlineData(1.11, 1, 0.01, 0.1, 1)]
        [InlineData(1.19, 1, 0.01, 0.1, 1)]
        [InlineData(1, 1, 0.01, 0.1, 1)]
        public async void SignalBuySizeShouldReturnExpectedResultWhenAboveMinQuantity(decimal balance,
                                                                                decimal price,
                                                                                decimal riskPercent,
                                                                                decimal stopPercent,
                                                                                decimal expectedResult)
        {
            // ARRANGE
            var lotSize = new SymbolLotSizeFilter()
            {
                MaxQuantity = 100,
                MinQuantity = 1,
                StepSize = 0.1m
            };

            var spotMarket = new Mock<IMarketConnection>();
            spotMarket.Setup(m => m.GetAvgPriceAsync(It.IsAny<Exchange>(), It.IsAny<string>())).Returns(Task.Run(() => price));
            spotMarket.Setup(m => m.GetBalanceAsync(It.IsAny<Exchange>(), "USDT")).Returns(Task.Run(() => balance));
            spotMarket.Setup(m => m.GetLotSizeAsync(It.IsAny<Exchange>(), "ETHUSDT")).Returns(Task.Run(() => lotSize));

            var config = new Mock<IOptions<ConfigurationOptions>>();
            config.Setup(m => m.Value.SignalRiskPercent).Returns(riskPercent);

            var subject = new SignalPositionSizer(config.Object, spotMarket.Object);

            // ACT
            var result = await subject.GetSignalBuySizeAsync(Exchange.Binance, stopPercent, "ETHUSDT");

            // ASSERT
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1.5, 1.5)]
        [InlineData(1.56, 1.5)]
        [InlineData(1.54, 1.5)]
        [InlineData(100, 100)]
        [InlineData(101, 100)]
        public async void SignalSellSizeShouldReturnExpectedResult(decimal balance, decimal expectedResult)
        {
            // ARRANGE
            var lotSize = new SymbolLotSizeFilter()
            {
                MaxQuantity = 100,
                MinQuantity = 1,
                StepSize = 0.1m
            };

            var spotInfo = new Mock<IMarketConnection>();
            spotInfo.Setup(m => m.GetBalanceAsync(It.IsAny<Exchange>(), "ETH")).Returns(Task.Run(() => balance));
            spotInfo.Setup(m => m.GetLotSizeAsync(It.IsAny<Exchange>(), "ETHUSDT")).Returns(Task.Run(() => lotSize));

            var config = new Mock<IOptions<ConfigurationOptions>>();
            config.Setup(m => m.Value.IsProduction).Returns(true);

            var subject = new SignalPositionSizer(config.Object, spotInfo.Object);

            // ACT
            var result = await subject.GetSignalSellSizeAsync(Exchange.Binance, "ETHUSDT", "60");

            // ASSERT
            result.ShouldBe(expectedResult);
        }
    }
}
