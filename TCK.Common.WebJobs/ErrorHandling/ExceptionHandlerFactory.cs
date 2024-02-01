namespace TCK.Common.WebJobs.ErrorHandling
{
    // Poor man's DI since the built-in container doesn't support property injection
    internal sealed class ExceptionHandlerFactory
    {
        public static ExceptionHandler CreateExceptionHandler()
        {
            return new ExceptionHandler(new ProblemDetailsFactory());
        }
    }
}
