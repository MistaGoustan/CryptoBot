using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using TCK.Common.WebJobs.ErrorHandling;

namespace TCK.Common.WebJobs
{
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        public ControllerBase(ILogger logger)
        {
            Logger = logger;
        }

        // Poor man's DI since the built-in container doesn't support property injection
        protected IExceptionHandler ExceptionHandler { get; set; } = ExceptionHandlerFactory.CreateExceptionHandler();

        protected ILogger Logger { get; set; }

        protected IActionResult ErrorHandler(Func<IActionResult> action)
        {
            try
            {
                return action.Invoke();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while procecssing the request.");

                return ExceptionHandler.CreateErrorResponse(ex);
            }
        }

        protected async Task<IActionResult> ErrorHandlerAsync(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action.Invoke();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while procecssing the request.");

                return ExceptionHandler.CreateErrorResponse(ex);
            }
        }

        protected IActionResult NotAuthorized() => new StatusCodeResult((Int32)HttpStatusCode.Unauthorized);

        protected IActionResult Ok(Object value) => new OkObjectResult(value);
    }
}
