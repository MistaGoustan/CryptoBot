// TODO FIX TESTS
//using Shouldly;
//using System;
//using System.Net;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Xunit;

//namespace TCK.Bot.Api.Test.IntegrationTests
//{
//    public class DynamicTradesControllerTests : IClassFixture<ApiFixture>, IDisposable
//    {
//        private readonly ApiFixture _fixture;
//        private const String _requestUri = "api/dynamic-trades";
//        private const String _ticker = "USDTUSD";

//        public DynamicTradesControllerTests(ApiFixture fixture)
//        {
//            _fixture = fixture;
//        }

//        public void Dispose()
//        {
//            _fixture.DynamicOrderRepository.DeleteOrdersWithTicker(_ticker);
//            _fixture.DynamicIsolatedWalletRepository.DeleteWalletWithTicker(_ticker);
//        }

//        [Fact]
//        public async Task ShouldReturnForbiddenWhenNotAuthorized()
//        {
//            // ARRANGE
//            var request = new DynamicTradeRequest();
//            var client = _fixture.CreateClient(false);

//            // ACT
//            var response = await client.PostAsJsonAsync(_requestUri, request);

//            // ASSERT
//            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
//        }

//        // TODO uncomment this test

//        //[Fact]
//        //public async Task ShouldReturnSuccessWhenRequested()
//        //{
//        //    // ARRANGE
//        //    var request = new DynamicTradeRequest
//        //    {
//        //        IsWeighted = false,
//        //        NumberOfOrders = 3,
//        //        UpperTargetPrice = 1.1m,
//        //        LowerTargetPrice = 1.01m,
//        //        UpperBuyPrice = 1,
//        //        LowerBuyPrice = 0.99m,
//        //        StopPrice = 0.8m,
//        //        Exchange = Exchange.Binance,
//        //        Ticker = _ticker,
//        //        PositionSide = PositionSide.Long
//        //    };

//        //    var client = _fixture.CreateClient(true);

//        //    // ACT
//        //    var response = await client.PostAsJsonAsync(_requestUri, request);

//        //    // ASSERT
//        //    response.StatusCode.ShouldBe(HttpStatusCode.OK);
//        //}
//    }
//}
