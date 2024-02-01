using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using TCK.Bot.Options;
using TCK.Bot.SignalService;

namespace TCK.Bot.Data
{
    internal class SignalOrderRepository : ISignalOrderRepository
    {
        private readonly String _connectionString;
        private readonly ILogger<SignalOrderRepository> _logger;
        private readonly ISignalPNLCalculator _pnl;

        public SignalOrderRepository(ILogger<SignalOrderRepository> logger, ISignalPNLCalculator pnl, IOptions<UrlOptions> urlOptions)
        {
            _logger = logger;
            _pnl = pnl;
            _connectionString = urlOptions.Value.Database;
        }

        public SignalOrder CreateOrder(SignalOrder order, OrderSide side)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "INSERT INTO [dbo].[SignalOrder] ([BuyDate], [BuyFee], [BuyOrderId], [BuyPrice], [Exchange], [FundingFee], [Interval], [PNL], [PositionSide], [Quantity], [SellDate], [SellFee], [SellOrderId], [SellPrice], [Status], [StopPrice], [Ticker]) " +
                "OUTPUT Inserted.[Id], Inserted.[BuyDate], Inserted.[BuyFee], Inserted.[BuyOrderId], Inserted.[BuyPrice], Inserted.[Exchange], Inserted.[FundingFee], Inserted.[Interval], Inserted.[PNL], Inserted.[PositionSide], Inserted.[Quantity], Inserted.[SellDate], Inserted.[SellFee], Inserted.[SellOrderId], Inserted.[SellPrice], Inserted.[Status], Inserted.[StopPrice], Inserted.[Ticker] " +
                "VALUES (@BuyDate, @BuyFee, @BuyOrderId, @BuyPrice, @Exchange, @FundingFee, @Interval, @PNL, @PositionSide, @Quantity, @SellDate, @SellFee, @SellOrderId, @SellPrice, @Status, @StopPrice, @Ticker)";

            return db.QuerySingleOrDefault<SignalOrder>(sql, new
            {
                BuyDate = order.BuyDate,
                BuyFee = order.BuyFee,
                BuyOrderId = order.BuyOrderId,
                BuyPrice = order.BuyPrice,
                Exchange = Enum.GetName(order.Exchange),
                FundingFee = order.FundingFee,
                Interval = order.Interval,
                PNL = 0,
                PositionSide = Enum.GetName(order.PositionSide),
                Quantity = order.Quantity,
                SellDate = (String)null!,
                SellFee = order.SellFee,
                SellOrderId = order.SellOrderId,
                SellPrice = order.SellPrice,
                Status = Enum.GetName(order.Status),
                StopPrice = order.StopPrice,
                Ticker = order.Ticker
            });
        }

        public async Task<SignalOrder?> GetInProgressOrderByIntervalAsync(Exchange exchange, String interval, String ticker)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT * FROM [dbo].[SignalOrder] " +
                "WHERE [Exchange] = @Exchange " +
                "AND [Interval] = @Interval " +
                "AND [Status] = @Status " +
                "AND [Ticker] = @Ticker ";

            var order =
                await db.QuerySingleOrDefaultAsync<SignalOrder>(sql, new
                {
                    Exchange = Enum.GetName(exchange),
                    Interval = interval,
                    Status = Enum.GetName(SignalOrderStatus.InProgress),
                    Ticker = ticker,
                });

            return order;
        }

        public async Task<IEnumerable<SignalOrder>?> GetInProgressOrders()
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT * FROM [dbo].[SignalOrder] " +
                "WHERE [Status] = @Status";

            var order =
                await db.QueryAsync<SignalOrder>(sql, new
                {
                    Status = Enum.GetName(SignalOrderStatus.InProgress)
                });

            return order;
        }

        public async Task<IEnumerable<SignalOrder>> GetOrdersByIntervalAsync(String ticker, String interval)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT * FROM [dbo].[SignalOrder] " +
                "WHERE [Ticker] = @Ticker " +
                "AND [Interval] = @Interval";

            var orders = await db.QueryAsync<SignalOrder>(sql, new { @Ticker = ticker, @Interval = interval });

            return orders;
        }

        public async Task<SignalOrder?> GetOrderByOrderIdAsync(Exchange exchange, String orderId)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT TOP (1) * FROM [dbo].[SignalOrder] " +
                "WHERE [Exchange] = @exchange " +
                "AND ([BuyOrderId] = @orderId " +
                "OR [SellOrderId] = @orderId)";

            var order =
                await db.QuerySingleOrDefaultAsync<SignalOrder>(sql, new
                {
                    Exchange = Enum.GetName(exchange),
                    OrderId = orderId
                });

            return order;
        }

        public async Task<IEnumerable<SignalOrder>?> GetRecentOrdersAsync(Exchange exchange, Int16 numberOfOrders, String ticker)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT TOP (@NumberOfOrders) * " +
                "FROM [dbo].[SignalOrder] " +
                "WHERE [Ticker] = @Ticker " +
                "AND [Exchange] = @Exchange " +
                "ORDER BY [Id] DESC";

            var orders = await db.QueryAsync<SignalOrder>(sql, new
            {
                @Exchange = Enum.GetName(exchange),
                @NumberOfOrders = numberOfOrders,
                @Ticker = ticker
            });

            return orders;
        }

        public SignalOrder UpdateOrder(SignalOrder order, OrderSide side)
        {
            var db = new SqlConnection(_connectionString);

            if (side == OrderSide.Sell)
            {
                order.PNL = _pnl.ForOrder(order);
            }

            var sql =
                "UPDATE [dbo].[SignalOrder] " +
                "SET [BuyDate] = @BuyDate, [BuyFee] = @BuyFee, [BuyOrderId] = @BuyOrderId, [BuyPrice] = @BuyPrice, [Exchange] = @Exchange, [FundingFee] = @FundingFee, [Interval] = @Interval, [PNL] = @PNL, [PositionSide] = @PositionSide, [Quantity] = @Quantity, [SellDate] = @SellDate, [SellFee] = @SellFee, [SellOrderId] = @SellOrderId, [SellPrice] = @SellPrice, [Status] = @Status, [StopPrice] = @StopPrice, [Ticker] = @Ticker " +
                "OUTPUT Inserted.[Id], Inserted.[BuyDate], Inserted.[BuyFee], Inserted.[BuyOrderId], Inserted.[BuyPrice], Inserted.[Exchange], Inserted.[FundingFee], Inserted.[Interval], Inserted.[PNL], Inserted.[PositionSide], Inserted.[Quantity], Inserted.[SellDate], Inserted.[SellFee], Inserted.[SellOrderId], Inserted.[SellPrice], Inserted.[Status], Inserted.[StopPrice], Inserted.[Ticker] " +
                "WHERE [Id] = @Id";

            return db.QuerySingleOrDefault<SignalOrder>(sql, new
            {
                Id = order.Id,
                BuyDate = order.BuyDate,
                BuyFee = order.BuyFee,
                BuyOrderId = order.BuyOrderId,
                BuyPrice = order.BuyPrice,
                Exchange = Enum.GetName(order.Exchange),
                FundingFee = order.FundingFee,
                Interval = order.Interval,
                PNL = order.PNL,
                PositionSide = Enum.GetName(order.PositionSide),
                Quantity = order.Quantity,
                SellDate = order.SellDate,
                SellFee = order.SellFee,
                SellOrderId = order.SellOrderId,
                SellPrice = order.SellPrice,
                Status = Enum.GetName(order.Status),
                StopPrice = order.StopPrice,
                Ticker = order.Ticker,
            });
        }

        internal void DeleteOrdersWithTicker(String ticker)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "DELETE FROM [dbo].[SignalOrder] " +
                "WHERE Ticker = @Ticker";

            db.Execute(sql, new
            {
                ticker
            });
        }
    }
}
