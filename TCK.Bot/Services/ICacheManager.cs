namespace TCK.Bot.Services
{
    public interface ICacheManager
    {
        SignalOrder GetSignalOrder(Exchange exchange, string ticker);

        void RemoveSignalOrder(Exchange exchange, string ticker);

        void SetSignalOrder(SignalOrder order);
    }
}