namespace TCK.Bot.SignalService
{
    public interface ISignalPNLCalculator
    {
        decimal ForOrder(SignalOrder order);
    }
}
