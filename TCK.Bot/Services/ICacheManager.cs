namespace TCK.Bot.Services
{
    public interface ICacheManager
    {
        SignalOrder GetSignalOrder(Exchange exchange, String ticker);

        void RemoveSignalOrder(Exchange exchange, String ticker);

        void SetSignalOrder(SignalOrder order);
    }
}