namespace TCK.Bot.DynamicService
{
    public interface IDynamicPNLCalculator
    {
        Task ForOrdersAsync(DynamicOrder[] orders);
    }
}
