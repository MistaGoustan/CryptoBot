using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TCK.Bot.Api.Extensions;
using TCK.Bot.DynamicService;
using TCK.Bot.Options;

namespace TCK.Bot.Api
{
    public static class Program
    {
        public static IConfiguration Configuration { get; private set; } = default!;

        public static async Task Main()
        {
            var builder = WebApplication.CreateBuilder();

            AddCustomAuthentication(builder);

            builder.Services.AddApi();
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            AddMiddleware(app);

            LogStartup(app);

            await AddObserverAsync(app);

            app.Run();
        }

        public static void AddCustomAuthentication(WebApplicationBuilder builder)
        {
            var key = builder.Configuration.GetSection("BearerAuthenticationOptions:Key").Value;

            builder.Services
                .AddOptions<BearerAuthenticationOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("BearerAuthenticationOptions").Bind(settings);
                });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration.GetSection("BearerAuthenticationOptions:Issuer").Value,
                    ValidAudience = builder.Configuration.GetSection("BearerAuthenticationOptions:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration.GetSection("BearerAuthenticationOptions:Key").Value)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            // Register Policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RestrictIP", policy =>
                    policy.Requirements.Add(new IPRequirement(builder.Configuration.GetSection("BasicAuthenticationOptions:Whitelist").Value)));

            });

            // Register Handler
            builder.Services.AddSingleton<IAuthorizationHandler, IPAddressHandler>();
        }

        private static async Task AddObserverAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var observer = scope.ServiceProvider.GetService<ITradeObserver>()
                ?? throw new Exception($"{typeof(ITradeObserver)} is not registered in the DI container.");

            await observer.InitializeAsync();
        }

        private static WebApplication AddMiddleware(WebApplication app)
        {
            app.MapControllers();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment()) // setup HTTP pipeline
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }

        private static void LogStartup(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var telemetry = scope.ServiceProvider.GetService<TelemetryClient>()
                ?? throw new Exception($"{typeof(TelemetryClient)} is not registered in the DI container.");

            telemetry.TrackTrace("Starting Bot....", SeverityLevel.Information);
        }
    }
}
