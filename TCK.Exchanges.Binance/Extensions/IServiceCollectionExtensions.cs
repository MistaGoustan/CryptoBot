using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCK.Bot.Binance;
using TCK.Bot.Options;
using TCK.Common.DependencyInjection;
using TCK.Exchanges.Binance.MappingProfiles;

namespace TCK.Exchanges.Binance.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBinance(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapperProfile))
                    .AddMemoryCache()
                    .AddOptions<BinanceOptions>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection("BinanceOptions").Bind(settings);
                    });

            services
                    .AddDecorator<IBinanceOrderService, BinanceMockOrderService>(svc => svc.AddScoped<IBinanceOrderService, BinanceOrderService>())
                    .AddDecorator<IBinanceSpotMarketConnection, CacheBinanceSpotMarketConnection>(svc => svc.AddScoped<IBinanceSpotMarketConnection, BinanceSpotMarketConnection>())
                    .AddTransient<IBinanceFeeCalculator, BinanceFeeCalculator>()
                    .AddTransient<IBinanceUserStreamService, BinanceUserStreamService>()
                    .AddTransient<IBinanceTickerSubscriber, BinanceTickerSubscriber>()
                    ;

            return services;
        }
    }
}
