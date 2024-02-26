using Binance.Net.Objects.Models.Spot;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCK.Bot;
using TCK.Bot.Services;
using Xunit;

namespace TCK.Exchanges.Binance.Test
{
    public sealed class BinanceFeeCalculatorTests
    {
        private readonly Mock<IMarketConnection> _market;
        private readonly BinanceFeeCalculator _subject;

        public BinanceFeeCalculatorTests()
        {
            _market = new Mock<IMarketConnection>();
            _subject = new BinanceFeeCalculator(_market.Object);
        }

        [Fact]
        public async void FeeCalculatorShouldReturnExpectedResultWhenAssetIsHappyPathValue()
        {
            // ARRANGE
            var asset = "ETH";
            var fee = 0.00000350m;
            var price = 4000m;
            var expectedResult = 0.028m;

            var trades = CreateTrades(asset, fee, price);

            // ACT
            var result = await _subject.GetTotaledFeeAsync(trades);

            // ASSERT
            result.ShouldBe(expectedResult);
        }

        [Fact]
        public async void FeeCalculatorShouldReturnExpectedResultWhenFeeTypeIsBNB()
        {
            // ARRANGE
            var asset = "BNB";
            var fee = 0.01m;
            var expectedResult = 8m;
            var bnbPrice = 400m;

            _market.Setup(m => m.GetAvgPriceAsync(Exchange.Binance, $"{asset}USDT")).Returns(Task.FromResult(bnbPrice));

            var trades = CreateTrades(asset, fee, 0);

            // ACT
            var result = await _subject.GetTotaledFeeAsync(trades);

            // ASSERT
            result.ShouldBe(expectedResult);
        }

        [Fact]
        public async void FeeCalculatorShouldReturnExpectedResultWhenFeeTypeIsUSDT()
        {
            // ARRANGE
            var asset = "USDT";
            var fee = 0.1m;
            var expectedResult = 0.2m;

            var trades = CreateTrades(asset, fee, 0);

            // ACT
            var result = await _subject.GetTotaledFeeAsync(trades);

            // ASSERT
            result.ShouldBe(expectedResult);
        }

        [Fact]
        public async void FeeCalculatorShouldReturnExpectedResultWhenFeeTypesIsALTAlsoBNB()
        {
            // ARRANGE
            var bnbPrice = 400m;
            var ethPrice = 4000m;

            _market.Setup(m => m.GetAvgPriceAsync(Exchange.Binance, "BNBUSDT")).Returns(Task.FromResult(bnbPrice));

            var trades = new List<BinanceOrderTrade>
            {
                new BinanceOrderTrade
                {
                    FeeAsset = "ETH",
                    Fee = 0.01m,
                    Price = ethPrice
                },
                new BinanceOrderTrade
                {
                    FeeAsset = "BNB",
                    Fee = 0.001m,
                    Price = ethPrice
                }
            };

            // ACT
            var result = await _subject.GetTotaledFeeAsync(trades);

            // ASSERT
            result.ShouldBe(40.4m);
        }

        [Fact]
        public async void FeeCalculatorShouldReturnExpectedResultWhenFeeTypeIsUSDTAlsoBNB()
        {
            // ARRANGE
            var bnbPrice = 400m;

            _market.Setup(m => m.GetAvgPriceAsync(Exchange.Binance, "BNBUSDT")).Returns(Task.FromResult(bnbPrice));

            var trades = new List<BinanceOrderTrade>
            {
                new BinanceOrderTrade
                {
                    FeeAsset = "USDT",
                    Fee = 1m
                },
                new BinanceOrderTrade
                {
                    FeeAsset = "BNB",
                    Fee = 0.001m
                }
            };

            // ACT
            var result = await _subject.GetTotaledFeeAsync(trades);

            // ASSERT
            result.ShouldBe(1.4m);
        }

        private static List<BinanceOrderTrade> CreateTrades(string asset, decimal fee, decimal price)
            => new()
            {
                new BinanceOrderTrade
                {
                    FeeAsset = asset,
                    Fee = fee,
                    Price = price,
                },
                new BinanceOrderTrade
                {
                    FeeAsset = asset,
                    Fee = fee,
                    Price = price,
                }
            };
    }
}
