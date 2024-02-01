namespace TCK.Bot.DynamicService
{
    public interface IDynamicOrderCache
    {
        void AddGroup(DynamicOrder[] orders);

        DynamicOrder[] GetGroup(String ticker);

        List<DynamicOrder[]>? GetAllGroups();

        DynamicOrder[]? GetGroupOrDefault(String ticker);

        void RemoveGroup(String ticker);
    }
}
