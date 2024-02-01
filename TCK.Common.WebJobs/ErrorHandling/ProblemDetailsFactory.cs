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

        private static String FormatTitle(Exception ex)
        {
            if (ex.InnerException is null)
                return ex.Message;

            return $"{ex.Message} (InnerException: {ex.InnerException})";
        }

        private static Int32 GetStatusCode(Exception ex)
        {
            if (ex is BadRequestException)
            {
                return (Int32)HttpStatusCode.BadRequest; // 400
            }

            //if (ex is SecurityTokenException)
            //{
            //    return (Int32)HttpStatusCode.Unauthorized; // 401
            //}

            return (Int32)HttpStatusCode.InternalServerError; // 500
        }
    }
}
