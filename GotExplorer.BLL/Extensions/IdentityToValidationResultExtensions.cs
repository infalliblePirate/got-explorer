using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace GotExplorer.BLL.Extensions
{
    public static class IdentityToValidationResultExtensions
    {
        public static ValidationResult ToValidationResult(this IdentityResult identityResult, string errorCode)
        {
            var validationResult = new ValidationResult();

            foreach (var error in identityResult.Errors)
            {
                validationResult.Errors.Add(new ValidationFailure(error.Code, error.Description) { ErrorCode=errorCode });
            }

            return validationResult;
        }
    }
}
