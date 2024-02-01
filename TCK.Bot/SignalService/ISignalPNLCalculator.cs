namespace TCK.Bot.SignalService
{
    public interface ISignalPNLCalculator
    {
        Decimal ForOrder(SignalOrder order);
    }
}
