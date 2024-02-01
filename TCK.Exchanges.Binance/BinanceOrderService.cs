using AutoMapper;
using Binance.Net.Enums;
using Microsoft.Extensions.Options;
using TCK.Bot;
using TCK.Bot.Binance;
using TCK.Bot.Options;
using TCK.Common.WebJobs;
using BinanceEnums = Binance.Net.Enums;

namespace TCK.Exchanges.Binance
{
    public class BinanceOrderService : BinanceBase, IBinanceOrderService
    {
        private readonly IBinanceFeeCalculator _feeCalculator;
        private readonly IMapper _mapper;

        public BinanceOrderService(IBinanceFeeCalculator feeCalculator,
                                   IMapper mapper,
                                   IOptions<BinanceOptions> options) : base(options)
        {
            _feeCalculator = feeCalculator;
            _mapper = mapper;
        }

        public async Task<PlacedOrder> PlaceMarketBuyAsync(Decimal price, Decimal quantity, String ticker)
        {
            var result = await Client.SpotApi.Trading.PlaceOrderAsync(ticker,
                                                                 _mapper.Map<BinanceEnums.OrderSide>(TCK.Bot.OrderSide.Buy),
                                                                 SpotOrderType.Market,
                                                                 quantity);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Fee = await _feeCalculator.GetTotaledFeeAsync(result.Data.Trades),
                OrderId = result.Data.Id.ToString(),
                Price = result.Data.AverageFillPrice ?? throw new Exception("Binance gave null fill price."),
                Quantity = result.Data.Quantity
            };
        }

        public async Task<PlacedOrder> PlaceMarketSellAsync(Decimal price, Decimal quantity, String ticker)
        {
            var result = await Client.SpotApi.Trading.PlaceOrderAsync(ticker,
                                                                 _mapper.Map<BinanceEnums.OrderSide>(TCK.Bot.OrderSide.Sell),
                                                                 SpotOrderType.Market,
                                                                 quantity);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Fee = await _feeCalculator.GetTotaledFeeAsync(result.Data.Trades),
                OrderId = result.Data.Id.ToString(),
                Price = result.Data.AverageFillPrice ?? throw new Exception("Binance gave null fill price."),
                Quantity = result.Data.Quantity
            };
        }


        public async Task<PlacedOrder> PlaceLimitBuyAsync(String ticker, Decimal quantity, Decimal price)
        {
            var result = await Client.SpotApi.Trading.PlaceOrderAsync(ticker,
                                                                 _mapper.Map<BinanceEnums.OrderSide>(TCK.Bot.OrderSide.Buy),
                                                                 SpotOrderType.Limit,
                                                                 quantity,
                                                                 price,
                                                                 timeInForce: TimeInForce.GoodTillCanceled);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Fee = await _feeCalculator.GetTotaledFeeAsync(result.Data.Trades),
                OrderId = result.Data.Id.ToString(),
                Price = result.Data.AverageFillPrice ?? throw new Exception("Binance gave null fill price."),
                Quantity = result.Data.Quantity
            };
        }

        public async Task<PlacedOrder> PlaceLimitSellAsync(String ticker, Decimal quantity, Decimal price)
        {
            var result = await Client.SpotApi.Trading.PlaceOrderAsync(ticker,
                                                                 _mapper.Map<BinanceEnums.OrderSide>(TCK.Bot.OrderSide.Sell),
                                                                 SpotOrderType.Limit,
                                                                 quantity,
                                                                 price,
                                                                 timeInForce: TimeInForce.GoodTillCanceled);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Fee = await _feeCalculator.GetTotaledFeeAsync(result.Data.Trades),
                OrderId = result.Data.Id.ToString(),
                Price = result.Data.AverageFillPrice ?? throw new Exception("Binance gave null fill price."),
                Quantity = result.Data.Quantity
            };
        }
    }
}
