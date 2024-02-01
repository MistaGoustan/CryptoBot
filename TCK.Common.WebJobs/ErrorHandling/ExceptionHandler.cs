using Microsoft.AspNetCore.Mvc;

namespace TCK.Common.WebJobs.ErrorHandling
{
    internal sealed class ExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsFactory _problemFactory;

        public ExceptionHandler(IProblemDetailsFactory problemFactory)
        {
            _problemFactory = problemFactory;
        }

        public IActionResult CreateErrorResponse(Exception ex)
        {
            var details = _problemFactory.CreateProblemDetails(ex);

            return new ObjectResult(details)
            {
                StatusCode = details.Status
            };
        }
    }
}
