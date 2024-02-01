using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using TCK.Bot.Options;

namespace TCK.Bot.Data.Test
{
    public class DatabaseFixture
    {
        internal readonly DynamicOrderRepository DynamicOrderRepository;

        public DatabaseFixture()
        {
            var services = CreateServices();

            var urlOptions = CreateIOptionsUrlOptions(services);

            DynamicOrderRepository = new DynamicOrderRepository(urlOptions);
        }

        private IOptions<UrlOptions> CreateIOptionsUrlOptions(IServiceProvider services)
        {
            var urlOptions = services.GetService<IOptions<UrlOptions>>()?.Value ?? throw new ArgumentNullException("Could not get UrlOptions.");

            var urlMock = new Mock<IOptions<UrlOptions>>();

            urlMock.SetupGet(x => x.Value).Returns(urlOptions);

            return urlMock.Object;
        }

        private ServiceProvider CreateServices()
        {
            var collection = new ServiceCollection();
            collection.AddOptions();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", false, false)
                .AddEnvironmentVariables()
                .Build();

            collection.Configure<UrlOptions>(config.GetSection("UrlOptions"));

            return collection.BuildServiceProvider();
        }
    }
}