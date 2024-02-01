// TODO FIX TESTS
//using Shouldly;
//using System;
//using System.Net;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Xunit;

//namespace TCK.Bot.Api.Test.IntegrationTests
//{
//    public class SignalTradesControllerTests : IClassFixture<ApiFixture>, IDisposable
//    {
//        private readonly ApiFixture _fixture;
//        private const String _requestUri = "api/signal-trades";
//        private const String _ticker = "USDTUSD";

//        public SignalTradesControllerTests(ApiFixture fixture)
//        {
//            _fixture = fixture;
//        }

//        public void Dispose()
//        {
//            _fixture.SignalOrderRepository.DeleteOrdersWithTicker(_ticker);
//            _fixture.SignalIsolatedWalletRepository.DeleteWalletWithTicker(_ticker);
//        }

//        [Fact]
//        public async Task ShouldReturnForbiddenWhenNotAuthorized()
//        {
//            // ARRANGE
//            var request = new SignalTradeRequest();
//            var client = _fixture.CreateClient(false);

//            // ACT
//            var response = await client.PostAsJsonAsync(_requestUri, request);

//            // ASSERT
//            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
//        }

//        // TODO: fix this test
//        //[Fact]
//        //public async Task ShouldReturnSuccessWhenRequested()
//        //{
//        //    // ARRANGE
//        //    var buyRequest = new SignalTradeRequest
//        //    {
//        //        Exchange = Exchange.Binance,
//        //        Interval = "TEST",
//        //        OrderSide = OrderSide.Buy,
//        //        StopPercent = 0.05m,
//        //        Ticker = _ticker
//        //    };

//        //    var sellRequest = new SignalTradeRequest
//        //    {
//        //        Exchange = Exchange.Binance,
//        //        Interval = "TEST",
//        //        OrderSide = OrderSide.Sell,
//        //        Ticker = _ticker
//        //    };

//        //    var client = _fixture.CreateClient(true);

//        //    // ACT
//        //    var buyResponse = await client.PostAsJsonAsync(_requestUri, buyRequest);
//        //    var sellResponse = await client.PostAsJsonAsync(_requestUri, sellRequest);

//        //    // ASSERT
//        //    buyResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
//        //    sellResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
//        //}
//    }
//}
