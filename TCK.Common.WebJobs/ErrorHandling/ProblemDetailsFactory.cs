using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TCK.Common.WebJobs.ErrorHandling
{
    internal sealed class ProblemDetailsFactory : IProblemDetailsFactory
    {
        public ProblemDetails CreateProblemDetails(Exception ex)
        {
            var status = GetStatusCode(ex);

            var data = new ProblemDetails()
            {
                Detail = $"StackTrace:{ex.StackTrace}",
                Status = status,
                Title = FormatTitle(ex),
                Type = ex.HelpLink,
            };

            return data;
        }

        private static string FormatTitle(Exception ex)
        {
            if (ex.InnerException is null)
                return ex.Message;

            return $"{ex.Message} (InnerException: {ex.InnerException})";
        }

        private static int GetStatusCode(Exception ex)
        {
            if (ex is BadRequestException)
            {
                return (int)HttpStatusCode.BadRequest; // 400
            }

            //if (ex is SecurityTokenException)
            //{
            //    return (int)HttpStatusCode.Unauthorized; // 401
            //}

            return (int)HttpStatusCode.InternalServerError; // 500
        }
    }
}
