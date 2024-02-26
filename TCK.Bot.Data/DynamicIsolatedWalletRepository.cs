using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using TCK.Bot.Options;
using TCK.Bot.Services;

namespace TCK.Bot.Data
{
    internal sealed class DynamicIsolatedWalletRepository : IDynamicIsolatedWalletRepository
    {
        private readonly IDbConnection _db;
        private readonly decimal _defaultWalletSize;
        private readonly ITickerValidator _tickerValidator;
        private readonly TelemetryClient _telemetry;

        public DynamicIsolatedWalletRepository(ITickerValidator tickerValidator, IOptions<ConfigurationOptions> configOptions, IOptions<UrlOptions> urlOptions, TelemetryClient telemetry)
        {
            var conn = urlOptions.Value.Database;
            _db = new SqlConnection(conn);

            _tickerValidator = tickerValidator;
            _defaultWalletSize = configOptions.Value.DefaultWalletSize;
            _telemetry = telemetry;
        }

        public void DeleteWalletWithTicker(string ticker)
        {
            var sql =
                "DELETE FROM [dbo].[DynamicIsolatedWallet] " +
                "WHERE Ticker = @Ticker";

            _db.Execute(sql, new
            {
                ticker
            });
        }

        public async Task<DynamicIsolatedWallet> GetWalletAsync(Exchange exchange, string ticker)
        {
            var sql =
                "SELECT * FROM [dbo].[DynamicIsolatedWallet] " +
                "WHERE Ticker = @Ticker";

            var wallet = _db.QuerySingleOrDefault<DynamicIsolatedWallet>(sql, new { Ticker = ticker });

            if (wallet is null)
            {
                var newWallet = await CreateWalletAsync(exchange, ticker);
                return newWallet;
            }

            return wallet;
        }

        public DynamicIsolatedWallet UpdateWallet(DynamicIsolatedWallet wallet)
        {
            var sql =
                "UPDATE [dbo].[DynamicIsolatedWallet] " +
                "SET AvailableBalance = @AvailableBalance, Balance = @Balance " +
                "OUTPUT Inserted.[AvailableBalance], Inserted.[Balance], Inserted.[Ticker] " +
                "WHERE Ticker = @Ticker";

            var updatedWallet = _db.QuerySingleOrDefault<DynamicIsolatedWallet>(sql, new
            {
                AvailableBalance = wallet.AvailableBalance,
                Balance = wallet.Balance,
                Ticker = wallet.Ticker
            });

            return updatedWallet;
        }

        private async Task<DynamicIsolatedWallet> CreateWalletAsync(Exchange exchange, string ticker)
        {
            if (!await _tickerValidator.TickerPairExistsAsync(exchange, ticker))
                throw new Exception($"{nameof(DynamicIsolatedWallet)} creation exception: Ticker pair does not exist on Binance");

            var newWallet = new DynamicIsolatedWallet
            {
                AvailableBalance = _defaultWalletSize,
                Balance = _defaultWalletSize,
                Ticker = ticker
            };

            var sql =
                "INSERT INTO [dbo].[DynamicIsolatedWallet] " +
                "VALUES (@AvailableBalance, @Balance, @Ticker)";

            _db.Execute(sql, newWallet);

            _telemetry.TrackTrace($"New mock {nameof(DynamicIsolatedWallet)} created.", SeverityLevel.Information);

            return newWallet;
        }
    }
}
