using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TCK.Bot.Options;
using TCK.Bot.Services;
using TCK.Common.DependencyInjection;

namespace TCK.Bot.Data.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddOptions<UrlOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("UrlOptions").Bind(settings);
                });

            services.AddDecorator<ISignalOrderRepository, MockSignalOrderRepository>(svc => svc.AddScoped<ISignalOrderRepository, SignalOrderRepository>())
                    .AddTransient<IDynamicIsolatedWalletRepository, DynamicIsolatedWalletRepository>()
                    .AddTransient<IDynamicOrderRepository, DynamicOrderRepository>()
                    .AddTransient<IIsolatedWalletService, IsolatedWalletService>()
                    .AddTransient<ISignalIsolatedWalletRepository, SignalIsolatedWalletRepository>()
                    ;

            return services;
        }
    }
}
