using AutoMapper;
using Microsoft.Extensions.Options;
using TCK.Bot;
using TCK.Bot.Binance;
using TCK.Bot.Options;
using TCK.Common.WebJobs;
using BinanceEnums = Binance.Net.Enums;

namespace TCK.Exchanges.Binance
{
    public sealed class BinanceMockOrderService : BinanceBase, IBinanceOrderService
    {
        private readonly ConfigurationOptions _configuration;
        private readonly IBinanceOrderService _innerService;
        private readonly IMapper _mapper;
        private readonly IBinanceSpotMarketConnection _market;

        public BinanceMockOrderService(IOptions<BinanceOptions> binanceOptions,
                                       IOptions<ConfigurationOptions> configOptions,
                                       IBinanceOrderService innerService,
                                       IMapper mapper,
                                       IBinanceSpotMarketConnection market)
            : base(binanceOptions)
        {
            _configuration = configOptions.Value;
            _innerService = innerService;
            _mapper = mapper;
            _market = market;
        }

        public async Task<PlacedOrder> PlaceLimitBuyAsync(String ticker, Decimal quantity, Decimal price)
        {
            if (_configuration.IsProduction)
            {
                return await _innerService.PlaceLimitBuyAsync(ticker, quantity, price);
            }

            var result = await Client.SpotApi.Trading.PlaceTestOrderAsync(ticker,
                                                                     _mapper.Map<BinanceEnums.OrderSide>(OrderSide.Buy),
                                                                     BinanceEnums.SpotOrderType.Limit,
                                                                     quantity,
                                                                     price,
                                                                     timeInForce: BinanceEnums.TimeInForce.GoodTillCanceled);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Price = price,
                Fee = _configuration.BuyFee,
                Quantity = quantity
            };
        }

        public async Task<PlacedOrder> PlaceLimitSellAsync(String ticker, Decimal quantity, Decimal price)
        {
            if (_configuration.IsProduction)
            {
                return await _innerService.PlaceLimitSellAsync(ticker, quantity, price);
            }

            var result = await Client.SpotApi.Trading.PlaceTestOrderAsync(ticker,
                                                                     _mapper.Map<BinanceEnums.OrderSide>(OrderSide.Sell),
                                                                     BinanceEnums.SpotOrderType.Limit,
                                                                     quantity,
                                                                     price,
                                                                     timeInForce: BinanceEnums.TimeInForce.GoodTillCanceled);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Price = price,
                Fee = _configuration.SellFee,
                Quantity = quantity
            };
        }

        public async Task<PlacedOrder> PlaceMarketBuyAsync(Decimal price, Decimal quantity, String ticker)
        {
            if (_configuration.IsProduction)
            {
                return await _innerService.PlaceMarketBuyAsync(price, quantity, ticker);
            }

            var result = await Client.SpotApi.Trading.PlaceTestOrderAsync(ticker,
                                                                          _mapper.Map<BinanceEnums.OrderSide>(OrderSide.Buy),
                                                                          BinanceEnums.SpotOrderType.Market,
                                                                          quantity);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Price = _configuration.HasLiveTestPrices ? await _market.GetAvgPriceAsync(ticker) : price,
                Fee = _configuration.BuyFee,
                Quantity = quantity
            };
        }

        public async Task<PlacedOrder> PlaceMarketSellAsync(Decimal price, Decimal quantity, String ticker)
        {
            if (_configuration.IsProduction)
            {
                return await _innerService.PlaceMarketSellAsync(price, quantity, ticker);
            }

            var result = await Client.SpotApi.Trading.PlaceTestOrderAsync(ticker,
                                                                          _mapper.Map<BinanceEnums.OrderSide>(OrderSide.Sell),
                                                                          BinanceEnums.SpotOrderType.Market,
                                                                          quantity);

            if (!result.Success)
            {
                throw new BadRequestException($"Bad Binance Request: {result.Error?.Message}");
            }

            return new PlacedOrder
            {
                Price = _configuration.HasLiveTestPrices ? await _market.GetAvgPriceAsync(ticker) : price,
                Fee = _configuration.SellFee,
                Quantity = quantity
            };
        }
    }
}
