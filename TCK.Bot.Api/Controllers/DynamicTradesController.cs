using Microsoft.AspNetCore.Mvc;
using TCK.Bot.Api.Extensions;
using TCK.Bot.DynamicService;

namespace TCK.Bot.Api.Controllers
{
    [ApiController]
    [Route("api/dynamic-trades")]
    public sealed class DynamicTradesController : Common.WebJobs.ControllerBase
    {
        private readonly IDynamicOrderCache _orderCache;
        private readonly IDynamicSubscriptionCache _subscriptionCache;
        private readonly IDynamicTrade _tradeService;

        public DynamicTradesController(ILogger<DynamicTradesController> logger,
                                       IDynamicOrderCache orderCache,
                                       IDynamicSubscriptionCache subscriptionCache,
                                       IDynamicTrade tradeSerivce)
            : base(logger)
        {
            _orderCache = orderCache;
            _subscriptionCache = subscriptionCache;
            _tradeService = tradeSerivce;
        }

        //[Authorize]
        [HttpDelete(Name = "CancelDynamicTrades")]
        [ProducesResponseType(typeof(DynamicOrder[]), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public Task<IActionResult> Cancel(CancelDynamicTradeRequest request) =>
            ErrorHandlerAsync(() => CancelDynamicTradeAsync(request));

        //[Authorize]
        [HttpPost(Name = "CreateDynamicTrades")]
        [ProducesResponseType(typeof(DynamicOrder[]), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public async Task<IActionResult> Run(DynamicTradeRequest request) =>
            await ErrorHandlerAsync(() => CreateDynamicTradesAsync(request));

        //[Authorize]
        [HttpPost("edit", Name = "EditDynamicTrades")]
        [ProducesResponseType(typeof(DynamicOrder[]), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public Task<IActionResult> Run(EditDynamicTradeRequest request) =>
            ErrorHandlerAsync(() => EditDynamicTradesAsync(request));

        //[Authorize]
        [HttpGet(Name = "GetDynamicTrades")]
        [ProducesResponseType(typeof(DynamicOrder[]), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public Task<IActionResult> Run(Exchange exchange, bool isDetailedTrades, string? ticker) =>
            ErrorHandlerAsync(() => GetDynamicTradesAsync(exchange, isDetailedTrades, ticker));

        //[Authorize]
        [HttpGet("cache", Name = "GetCache")]
        [ProducesResponseType(typeof(DynamicTradesCache), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest, "application/json")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError, "application/json")]
        public IActionResult Run() =>
            ErrorHandler(() => GetCache());

        private async Task<IActionResult> CancelDynamicTradeAsync(CancelDynamicTradeRequest request)
        {
            request.Validate();

            var orders = await _tradeService.CancelTradesAsync(request);

            return orders is null || !orders.Any() ?
                Ok($"No {request.Tickers} orders on {Enum.GetName(request.Exchange)} to cancel or complete.") :
                Ok(orders);
        }

        private async Task<IActionResult> CreateDynamicTradesAsync(DynamicTradeRequest request)
        {
            request.Validate();

            var existingTrades = _orderCache.GetGroupOrDefault(request.Ticker);

            return existingTrades is null || !existingTrades.Any() || existingTrades.All(t => t.Status is DynamicOrderStatus.Completed || t.Status is DynamicOrderStatus.Canceled) ?
                Ok(await _tradeService.ExecuteTradesAsync(request)) :
                Ok($"Trades already exist for {request.Ticker} on {request.Exchange}");
        }

        private async Task<IActionResult> GetDynamicTradesAsync(Exchange exchange, bool isDetailedTrades, string? ticker)
        {
            var trades = await _tradeService.GetTradesAsync(exchange, isDetailedTrades, ticker);

            return trades is null || !trades.Any() ?
                Ok($"No recent orders found for {ticker} on {Enum.GetName(exchange)}.") :
                Ok(trades);
        }

        private IActionResult GetCache()
        {
            var status = new DynamicTradesCache
            {
                Orders = _orderCache.GetAllGroups(),
                Subscriptions = _subscriptionCache.GetAll()
            };

            return Ok(status);
        }

        private async Task<IActionResult> EditDynamicTradesAsync(EditDynamicTradeRequest request)
        {
            var trades = await _tradeService.EditTradesAsync(request);

            return trades is null || !trades.Any() ?
                Ok($"No {request.Ticker} orders to edit on {Enum.GetName(request.Exchange)} or you are trying to increase your risk instead of reduce it.") :
                Ok(trades);
        }
    }
}
