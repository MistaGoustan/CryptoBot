namespace TCK.Bot.DynamicService
{
    public interface IDynamicOrderTerminator
    {
        Task<DynamicOrder[]> ForAsync(Exchange exchange, DynamicOrder[] orders);
    }
}