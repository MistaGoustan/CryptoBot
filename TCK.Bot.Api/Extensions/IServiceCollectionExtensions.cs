using System.Text.Json.Serialization;
using TCK.Bot.Api.Jobs;
using TCK.Bot.Data.Extensions;
using TCK.Bot.DynamicService.Extensions;
using TCK.Bot.Extensions;
using TCK.Bot.SignalService.Extensions;
using TCK.Exchanges.Binance.Extensions;

namespace TCK.Bot.Api.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services
                .AddMvc()
                .AddApplicationPart(typeof(Program).Assembly); // so itegration tests can use Program.cs

            services
                .AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services
                .AddCronJob<UserStreamConnectionJob>(c =>
                {
                    c.CronExpression = "0 */30 * * * *"; // "*/10 * * * * *";
                    c.TimeZoneInfo = TimeZoneInfo.Utc;
                });

            services
                .AddApplicationInsightsTelemetry()
                .AddBinance()
                .AddData()
                .AddDynamicService()
                .AddDomain()
                .AddEndpointsApiExplorer()
                .AddSignalService()
                .AddSwaggerGen();

            return services;
        }
    }
}
