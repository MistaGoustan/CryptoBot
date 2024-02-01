using Microsoft.Extensions.DependencyInjection;
using TCK.Common.DependencyInjection;


namespace TCK.Bot.SignalService.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSignalService(this IServiceCollection services)
        {
            services
                .AddDecorator<ISignalPositionSizer, MockSignalPositionSizer>(svc => svc.AddScoped<ISignalPositionSizer, SignalPositionSizer>())
                .AddTransient<ISignalPNLCalculator, SignalPNLCalculator>()
                .AddTransient<ISignalTrade, SignalTrade>()
                .AddTransient<ISignalTradeDecider, SignalTradeDecider>()
                .AddTransient<ISignalOrderAnalyzer, SignalOrderAnalyzer>();

            return services;
        }
    }
}
