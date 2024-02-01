using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCK.Bot.DynamicService;
using TCK.Bot.Options;
using TCK.Bot.Services;
using TCK.Bot.SignalService;
using TCK.Common.DependencyInjection;

namespace TCK.Bot.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddOptions<ConfigurationOptions>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection("ConfigurationOptions").Bind(settings);
                    });

            services
                .AddDecorator<IBalanceChecker, MockBalanceChecker>(svc => svc.AddScoped<IBalanceChecker, BalanceChecker>())
                .AddTransient<ICacheManager, CacheManager>()
                .AddTransient<IDynamicOrderRetriever, DynamicOrderRetriever>()
                .AddTransient<IDynamicOrderService, DynamicOrderService>()
                .AddTransient<IDynamicOrderUpdater, DynamicOrderUpdater>()
                .AddTransient<ILotSizeRetriever, LotSizeRetriever>()
                .AddTransient<IMarketConnection, MarketConnection>()
                .AddTransient<ISignalOrderService, SignalOrderService>()
                .AddTransient<ITickerSubscriber, TickerSubscriber>()
                .AddTransient<ITickerValidator, TickerValidator>()
                .AddTransient<IUserStreamer, UserStreamer>();

            return services;
        }
    }
}
