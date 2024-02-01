using Microsoft.Extensions.DependencyInjection;
using TCK.Common.DependencyInjection;

namespace TCK.Bot.DynamicService.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDynamicService(this IServiceCollection services)
        {
            services
                .AddDecorator<IDynamicPNLCalculator, MockDynamicPNLCalculator>(svc => svc.AddScoped<IDynamicPNLCalculator, DynamicPNLCalculator>())
                .AddDecorator<IDynamicPositionSizer, MockDynamicPositionSizer>(svc => svc.AddScoped<IDynamicPositionSizer, DynamicPositionSizer>())
                .AddDecorator<IDynamicTrade, MockDynamicTrade>(svc => svc.AddScoped<IDynamicTrade, DynamicTrade>())
                .AddSingleton<IDynamicOrderCache, DynamicOrderCache>()
                .AddSingleton<IDynamicSubscriptionCache, DynamicSubscriptionCache>()
                .AddTransient<IDynamicOrderAnalyzer, DynamicOrderAnalyzer>()
                .AddTransient<IDynamicOrderFactory, DynamicOrderFactory>()
                .AddTransient<IDynamicOrderTerminator, DynamicOrderTerminator>()
                .AddTransient<IDynamicIsolatedWalletProcessor, DynamicIsolatedWalletProcessor>()
                .AddTransient<IInProgressOrderAnalyzer, InProgressOrderAnalyzer>()
                .AddTransient<ITradeObserver, TradeObserver>()
                .AddTransient<IPendingOrderAnalyzer, PendingOrderAnalyzer>();

            return services;
        }
    }
}
