using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TCK.Bot.Api.Extensions;
using TCK.Bot.SignalService;

namespace TCK.Bot.Api.Controllers
{
    [ApiController]
    [Route("api/signal-trades")]
    public sealed class SignalTradesController : Common.WebJobs.ControllerBase
    {
        private readonly ISignalTrade _tradeSerivce;
        private readonly ISignalTradeDecider _tradeDecider;

        public SignalTradesController(ISignalTrade tradeService, ISignalTradeDecider tradeDecider, ILogger<SignalTradesController> logger) : base(logger)
        {
            _tradeSerivce = tradeService;
            _tradeDecider = tradeDecider;
        }

        [Authorize(Policy = "RestrictIP")]
        [HttpPost(Name = "CreateSignalTrade")]
        [ProducesResponseType(typeof(SignalOrder), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public async Task<IActionResult> Run(SignalTradeRequest request) =>
            await ErrorHandlerAsync(() => CreateSignalTradeAsync(request));

        //[Authorize]
        [HttpGet(Name = "GetSignalTrade")]
        [ProducesResponseType(typeof(SignalOrder), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public IActionResult Run(Exchange exchange, Boolean isDetailedTrades, Int16 numberOfOrders, String ticker) =>
            ErrorHandler(() => GetRecentSignalTradesAsync(exchange, isDetailedTrades, numberOfOrders, ticker));

        private async Task<IActionResult> CreateSignalTradeAsync(SignalTradeRequest request)
        {
            request.Validate();

            if (await _tradeDecider.CanTradeAsync(request.Interval,
                                                  request.OrderSide,
                                                  request.Ticker))
            {
                var result = await _tradeSerivce.TradeAsync(request);

                return Ok(result);
            }

            return Ok($"Unable to SignalTrade.");
        }

        private IActionResult GetRecentSignalTradesAsync(Exchange exchange, Boolean isDetailedTrades, Int16 numberOfOrders, String ticker)
        {
            var orders = _tradeSerivce.GetRecentOrdersAsync(exchange, isDetailedTrades, numberOfOrders, ticker);

            return orders is null ? Ok($"No recent orders found for {ticker} on {Enum.GetName(exchange)}.") : Ok(orders);
        }
    }
}
