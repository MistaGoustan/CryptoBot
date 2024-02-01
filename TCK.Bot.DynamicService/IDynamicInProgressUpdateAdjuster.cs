namespace TCK.Bot.DynamicService
{
    public interface IDynamicInProgressUpdateAdjuster
    {
        void ForOrders(DynamicOrder[] orders);
    }
}