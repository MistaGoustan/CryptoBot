// TODO FIX TESTS
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using System;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using TCK.Bot.Data;
//using TCK.Bot.Data.Extensions;
//using TCK.Bot.Extensions;
//using TCK.Bot.Options;

//namespace TCK.Bot.Api.Test.IntegrationTests
//{
//    public class ApiFixture
//    {
//        internal readonly IDynamicIsolatedWalletRepository DynamicIsolatedWalletRepository;
//        internal readonly DynamicOrderRepository DynamicOrderRepository;

//        internal readonly ISignalIsolatedWalletRepository SignalIsolatedWalletRepository;
//        internal readonly SignalOrderRepository SignalOrderRepository;

//        private readonly WebApplicationFactory<Program> _application;
//        private readonly BasicAuthenticationOptions _authenticationOptions;

//        public ApiFixture()
//        {
//            _application = CreateApplication();

//            _authenticationOptions = _application.Services.GetService<IOptions<BasicAuthenticationOptions>>()?.Value ?? throw new ArgumentNullException($"Could not get {nameof(Options.BasicAuthenticationOptions)}.");

//            DynamicOrderRepository = (DynamicOrderRepository)(_application.Services.GetService<IDynamicOrderRepository>() ?? throw new ArgumentNullException($"Could not get {nameof(DynamicOrderRepository)}."));
//            DynamicIsolatedWalletRepository = _application.Services.GetService<IDynamicIsolatedWalletRepository>() ?? throw new ArgumentNullException($"Could not get {nameof(IDynamicIsolatedWalletRepository)}.");

//            SignalOrderRepository = _application.Services.GetService<SignalOrderRepository>() ?? throw new ArgumentNullException($"Could not get {nameof(SignalOrderRepository)}.");
//            SignalIsolatedWalletRepository = _application.Services.GetService<ISignalIsolatedWalletRepository>() ?? throw new ArgumentNullException($"Could not get {nameof(ISignalIsolatedWalletRepository)}.");
//        }

//        private WebApplicationFactory<Program> CreateApplication()
//        {
//            var appFactory = new WebApplicationFactory<Program>()
//                .WithWebHostBuilder(builder =>
//                {
//                    builder.ConfigureServices(services =>
//                    {
//                        services
//                            .AddApplicationInsightsTelemetry()
//                            .AddData()
//                            .AddDomain();
//                    });
//                });

//            return appFactory;
//        }

//        public HttpClient CreateClient(bool isAutherized)
//        {
//            var client = _application.CreateClient();

//            if (isAutherized)
//            {
//                client.DefaultRequestHeaders.Authorization =
//                    new AuthenticationHeaderValue(_authenticationOptions.Scheme, _authenticationOptions.Parameter);
//            }

//            return client;
//        }
//    }
//}
