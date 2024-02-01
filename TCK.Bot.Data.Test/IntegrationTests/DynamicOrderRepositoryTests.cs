using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TCK.Bot.Data.Test.IntegrationTests
{
    public sealed class DynamicOrderRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly String _tickerTestName = "DOR/TEST";
        private readonly DynamicOrderRepository _subject;

        public DynamicOrderRepositoryTests(DatabaseFixture databaseFixture)
        {
            _subject = databaseFixture.DynamicOrderRepository;
        }

        public void Dispose()
        {
            _subject.DeleteOrdersWithTicker(_tickerTestName);
        }

        [Fact]
        public async Task GetUncompletedOrdersShouldNotReturnCompletedOrCanceledOrders()
        {
            // ARRANGE
            var unsavedOrders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    Status = DynamicOrderStatus.Pending,
                    Ticker = _tickerTestName
                },
                new DynamicOrder
                {
                    Status = DynamicOrderStatus.InProgress,
                    Ticker = _tickerTestName
                },
                new DynamicOrder
                {
                    Status = DynamicOrderStatus.Canceled,
                    Ticker = _tickerTestName
                },
                new DynamicOrder
                {
                    Status = DynamicOrderStatus.Completed,
                    Ticker = _tickerTestName
                }
            };

            _subject.SaveNewOrders(unsavedOrders);

            // ACT
            var result = await _subject.GetUncompletedOrdersAsync();

            // ASSERT
            foreach (var order in result)
            {
                order.Status.ShouldNotBe(DynamicOrderStatus.Canceled);
                order.Status.ShouldNotBe(DynamicOrderStatus.Completed);
            }
        }

        [Fact]
        public void SaveOrdersShouldSaveAllOrders()
        {
            // ARRANGE
            var orders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    Ticker = _tickerTestName
                },
                new DynamicOrder
                {
                    Ticker = _tickerTestName
                },
                new DynamicOrder
                {
                    Ticker = _tickerTestName
                }
            };

            // ACT
            var result = _subject.SaveNewOrders(orders);

            // ASSERT
            result.Length.ShouldBe(3);
        }

        [Fact]
        public async Task UpdateOrderShouldUpdateOrderAsExpected()
        {
            // ARRANGE
            var unsavedOrders = new DynamicOrder[]
            {
                new DynamicOrder
                {
                    Status = DynamicOrderStatus.Pending,
                    Ticker = _tickerTestName
                }
            };

            var savedOrder = _subject.SaveNewOrders(unsavedOrders)[0];

            savedOrder.Status = DynamicOrderStatus.InProgress;

            // ACT
            var updatedOrder = await _subject.UpdateOrderAsync(savedOrder);

            // ASSERT
            updatedOrder.Status.ShouldBe(DynamicOrderStatus.InProgress);
        }
    }
}