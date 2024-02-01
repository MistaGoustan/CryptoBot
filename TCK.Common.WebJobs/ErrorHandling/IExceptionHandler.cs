using Microsoft.AspNetCore.Mvc;

namespace TCK.Common.WebJobs.ErrorHandling
{
    public interface IExceptionHandler
    {
        IActionResult CreateErrorResponse(Exception ex);
    }
}
