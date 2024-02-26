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
    internal sealed class SignalIsolatedWalletRepository : ISignalIsolatedWalletRepository
    {
        private readonly IDbConnection _db;
        private readonly decimal _defaultWalletSize;
        private readonly ITickerValidator _tickerValidator;
        private readonly TelemetryClient _telemetry;

        public SignalIsolatedWalletRepository(ITickerValidator tickerValidator, IOptions<ConfigurationOptions> configOptions, IOptions<UrlOptions> urlOptions, TelemetryClient telemetry)
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
                "DELETE FROM [dbo].[SignalIsolatedWallet] " +
                "WHERE Ticker = @Ticker";

            _db.Execute(sql, new
            {
                ticker
            });
        }

        public async Task<decimal> GetBalanceAsync(Exchange exchange, string ticker)
        {
            var sql = "SELECT * FROM [dbo].[SignalIsolatedWallet] WHERE Ticker = @Ticker";

            var wallet = await _db.QuerySingleOrDefaultAsync<IsolatedWallet>(sql, new { Ticker = ticker });

            if (wallet is null)
            {
                var newWallet = await CreateWallet(exchange, ticker);
                return newWallet.Balance;
            }

            return wallet.Balance;
        }

        public async Task UpdateBalanceAsync(string ticker, decimal newBalance, decimal price)
        {
            var sql = "UPDATE [dbo].[SignalIsolatedWallet] SET Balance = @Balance WHERE Ticker = @Ticker";

            await _db.ExecuteAsync(sql, new
            {
                Balance = newBalance,
                Ticker = ticker
            });
        }

        private async Task<IsolatedWallet> CreateWallet(Exchange exchange, string ticker)
        {
            if (!await _tickerValidator.TickerPairExistsAsync(exchange, ticker))
                throw new Exception($"Signal Wallet creation exception: Ticker pair does not exist on {Enum.GetName(exchange)}.");

            var newWallet = new IsolatedWallet
            {
                Balance = _defaultWalletSize,
                Ticker = ticker
            };

            var sql = "INSERT INTO [dbo].[SignalIsolatedWallet] VALUES (@Balance, @Ticker)";

            await _db.ExecuteAsync(sql, newWallet);

            _telemetry.TrackTrace("New mock isolated wallet created.", SeverityLevel.Information);

            return newWallet;
        }
    }
}
