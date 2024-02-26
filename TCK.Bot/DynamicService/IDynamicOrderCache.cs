namespace TCK.Bot.DynamicService
{
    public interface IDynamicOrderCache
    {
        void AddGroup(DynamicOrder[] orders);

        DynamicOrder[] GetGroup(string ticker);

        List<DynamicOrder[]>? GetAllGroups();

        DynamicOrder[]? GetGroupOrDefault(string ticker);

        void RemoveGroup(string ticker);
    }
}
