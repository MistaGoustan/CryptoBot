using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using TCK.Bot.Options;

namespace TCK.Bot.Data
{
    internal sealed class DynamicOrderRepository : IDynamicOrderRepository
    {
        private readonly string _connectionString;

        public DynamicOrderRepository(IOptions<UrlOptions> urlOptions)
        {
            _connectionString = urlOptions.Value.Database;
        }

        public async Task<DynamicOrder[]?> GetActiveGroupedOrdersAsync(Exchange exchange)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT * FROM [dbo].[DynamicOrder] " +
                "WHERE [Exchange] = @Exchange " +
                "AND ([Status] = @InProgress " +
                "OR [Status] = @Pending)";

            var activeOrders =
                await db.QueryAsync<DynamicOrder>(sql, new
                {
                    Exchange = Enum.GetName(exchange),
                    InProgress = Enum.GetName(DynamicOrderStatus.InProgress),
                    Pending = Enum.GetName(DynamicOrderStatus.Pending)
                });

            var orderGroupIds = activeOrders.Select(o => o.OrderGroupId).Distinct();

            IEnumerable<DynamicOrder>? orderGroups = new List<DynamicOrder>();

            foreach (var id in orderGroupIds)
            {
                var orderGroup = await GetOrdersByOrderGroupIdAsync(id);

                if (orderGroup is not null)
                {
                    orderGroups = orderGroups.Concat(orderGroup);
                }
            }

            return orderGroups.ToArray();
        }

        public async Task<DynamicOrder[]?> GetRecentOrdersByTickerAsync(Exchange exchange, string ticker)
        {
            var order = await GetRecentOrderByTicker(exchange, ticker);

            if (order is null)
            {
                return null;
            }

            var orders = await GetOrdersByOrderGroupIdAsync(order.OrderGroupId);

            return orders?.ToArray() ?? null;
        }

        public async Task<IEnumerable<DynamicOrder>?> GetOrdersByOrderGroupIdAsync(string orderGroupId)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT * FROM [dbo].[DynamicOrder] " +
                "WHERE [OrderGroupId] = @OrderGroupId";

            var orders =
                await db.QueryAsync<DynamicOrder>(sql, new
                {
                    OrderGroupId = orderGroupId
                });

            return orders;
        }

        public async Task<DynamicOrder[]> GetUncompletedOrdersAsync()
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT * FROM [dbo].[DynamicOrder] " +
                "WHERE [Status] != @Completed AND [Status] != @Canceled";

            var orders =
                await db.QueryAsync<DynamicOrder>(sql, new
                {
                    Canceled = Enum.GetName(DynamicOrderStatus.Canceled),
                    Completed = Enum.GetName(DynamicOrderStatus.Completed)
                });

            return orders.ToArray();
        }

        public DynamicOrder[] SaveNewOrders(DynamicOrder[] orders)
        {
            Array.Reverse(orders); // this is just so the orders show up cosmetically in order in the DB

            var db = new SqlConnection(_connectionString);

            var sql =
                "INSERT INTO [dbo].[DynamicOrder] ([BuyDate], [BuyFee], [BuyOrderId], [BuyPrice], [Exchange], [FundingFee], [OrderGroupId], [PNL], [PositionSide], [QuantityFilled], [QuantityQuoted], [SellDate], [SellFee], [SellOrderId], [SellPrice], [Status], [StopPrice], [TargetPrice], [TargetQuantity], [Ticker]) " +
                "OUTPUT Inserted.[Id], Inserted.[BuyDate], Inserted.[BuyFee], Inserted.[BuyOrderId], Inserted.[BuyPrice], Inserted.[Exchange], Inserted.[FundingFee], Inserted.[OrderGroupId], Inserted.[PNL], Inserted.[PositionSide], Inserted.[QuantityFilled], Inserted.[QuantityQuoted], Inserted.[SellDate], Inserted.[SellFee], Inserted.[SellOrderId], Inserted.[SellPrice], Inserted.[Status], Inserted.[StopPrice], Inserted.[TargetPrice], Inserted.[TargetQuantity], Inserted.[Ticker] " +
                "VALUES (@BuyDate, @BuyFee, @BuyOrderId, @BuyPrice, @Exchange, @FundingFee, @OrderGroupId, @PNL, @PositionSide, @QuantityFilled, @QuantityQuoted, @SellDate, @SellFee, @SellOrderId, @SellPrice, @Status, @StopPrice, @TargetPrice, @TargetQuantity, @Ticker)";

            for (int i = 0; i < orders.Length; i++) // TODO: Performance Increase - Bulk Insert https://www.learndapper.com/bulk-operations/bulk-insert
            {
                orders[i] = db.QuerySingleOrDefault<DynamicOrder>(sql, new
                {
                    @BuyDate = DateTime.UtcNow,
                    orders[i].BuyFee,
                    orders[i].BuyOrderId,
                    orders[i].BuyPrice,
                    @Exchange = Enum.GetName(orders[i].Exchange),
                    orders[i].FundingFee,
                    orders[i].OrderGroupId,
                    orders[i].PNL,
                    @PositionSide = Enum.GetName(orders[i].PositionSide),
                    orders[i].QuantityFilled,
                    orders[i].QuantityQuoted,
                    orders[i].SellDate,
                    orders[i].SellFee,
                    orders[i].SellOrderId,
                    orders[i].SellPrice,
                    @Status = Enum.GetName(orders[i].Status),
                    orders[i].StopPrice,
                    orders[i].TargetPrice,
                    orders[i].TargetQuantity,
                    orders[i].Ticker
                });
            }

            return orders;
        }

        public Task<DynamicOrder> UpdateOrderAsync(DynamicOrder order)
        {
            var sql =
            "UPDATE [dbo].[DynamicOrder] " +
            "SET [BuyDate] = @BuyDate, [BuyFee] = @BuyFee, [BuyOrderId] = @BuyOrderId, [BuyPrice] = @BuyPrice, [Exchange] = @Exchange, [FundingFee] = @FundingFee, [OrderGroupId] = @OrderGroupId, [PNL] = @PNL, [PositionSide] = @PositionSide, [QuantityFilled] = @QuantityFilled, [QuantityQuoted] = @QuantityQuoted, [TargetQuantity] = @TargetQuantity, [SellDate] = @SellDate, [SellFee] = @SellFee, [SellOrderId] = @SellOrderId, [SellPrice] = @SellPrice, [Status] = @Status, [StopPrice] = @StopPrice, [TargetPrice] = @TargetPrice, [Ticker] = @Ticker " +
            "OUTPUT Inserted.[Id], Inserted.[BuyDate], Inserted.[BuyFee], Inserted.[BuyOrderId], Inserted.[BuyPrice], Inserted.[Exchange], Inserted.[FundingFee], Inserted.[OrderGroupId], Inserted.[PNL], Inserted.[PositionSide], Inserted.[QuantityFilled], Inserted.[QuantityQuoted], Inserted.[SellDate], Inserted.[SellFee], Inserted.[SellOrderId], Inserted.[SellPrice], Inserted.[Status], Inserted.[StopPrice], Inserted.[TargetPrice], Inserted.[TargetQuantity], Inserted.[Ticker] " +
            "WHERE [Id] = @Id";

            var db = new SqlConnection(_connectionString);

            var updatedOrder = db.QuerySingleOrDefaultAsync<DynamicOrder>(sql, new
            {
                order.BuyDate,
                order.BuyFee,
                order.BuyOrderId,
                order.BuyPrice,
                @Exchange = Enum.GetName(order.Exchange),
                order.FundingFee,
                order.OrderGroupId,
                order.Id,
                order.PNL,
                @PositionSide = Enum.GetName(order.PositionSide),
                order.QuantityFilled,
                order.QuantityQuoted,
                order.SellDate,
                order.SellFee,
                order.SellOrderId,
                order.SellPrice,
                @Status = Enum.GetName(order.Status),
                order.StopPrice,
                order.TargetPrice,
                order.TargetQuantity,
                order.Ticker
            });

            return updatedOrder;
        }

        public async Task<DynamicOrder[]> UpdateOrdersAsync(DynamicOrder[] orders)
        {
            for (int i = 0; i < orders.Length; i++) // TODO: Performance Increase - Bulk Insert https://www.learndapper.com/bulk-operations/bulk-insert
            {
                orders[i] = await UpdateOrderAsync(orders[i]);
            }

            return orders;
        }

        public Task<DynamicOrder> UpdateOrderAsync(DynamicOrder order, OrderSide side)
        {
            return UpdateOrderAsync(order);
        }

        public async Task<DynamicOrder[]> UpdateOrdersAsync(DynamicOrder[] orders, OrderSide side)
        {
            for (int i = 0; i < orders.Length; i++) // TODO: Performance Increase - Bulk Insert https://www.learndapper.com/bulk-operations/bulk-insert
            {
                orders[i] = await UpdateOrderAsync(orders[i]);
            }

            return orders;
        }

        internal void DeleteOrdersWithTicker(string ticker)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "DELETE FROM [dbo].[DynamicOrder] " +
                "WHERE Ticker = @Ticker";

            db.Execute(sql, new
            {
                ticker
            });
        }

        private async Task<DynamicOrder> GetRecentOrderByTicker(Exchange exchange, string ticker)
        {
            var db = new SqlConnection(_connectionString);

            var sql =
                "SELECT TOP (1) * FROM [dbo].[DynamicOrder] " +
                "WHERE [Ticker] = @Ticker " +
                "ORDER BY Id DESC";

            var order =
                await db.QuerySingleOrDefaultAsync<DynamicOrder>(sql, new
                {
                    Exchange = Enum.GetName(exchange),
                    Ticker = ticker
                });

            return order;
        }
    }
}