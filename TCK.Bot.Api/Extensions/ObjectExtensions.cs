using System.ComponentModel.DataAnnotations;
using TCK.Common.WebJobs;

namespace TCK.Bot.Api.Extensions
{
    internal static class ObjectExtensions
    {
        internal static void Validate(this Object o)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(o, new ValidationContext(o, null, null), validationResults, true);

            if (!isValid)
            {
                throw new BadRequestException($"{o.GetType().Name} is invalid: {String.Join(", ", validationResults.Select(s => s.ErrorMessage))}");
            }
        }
    }
}
