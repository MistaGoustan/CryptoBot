using Microsoft.AspNetCore.Mvc;

namespace TCK.Common.WebJobs.ErrorHandling
{
    internal interface IProblemDetailsFactory
    {
        ProblemDetails CreateProblemDetails(Exception ex);
    }
}