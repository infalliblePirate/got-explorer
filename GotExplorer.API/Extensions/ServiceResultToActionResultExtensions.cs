using GotExplorer.BLL.Services.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System;

namespace GotExplorer.API.Extensions
{
    public static class ServiceResultToActionResultExtensions
    {
        public static IActionResult ToActionResult<T>(this ServiceResult<T> serviceResult)
        {
            if (serviceResult.IsSuccess)
            {
                return new OkObjectResult(serviceResult.ResultObject);
            }
            return new ObjectResult(GetProblemDetails(serviceResult.Error));
        }
        private static ProblemDetails GetProblemDetails(Error error)
        {
            int code = error.Code switch
            {
                ErrorCodes.NotFound => StatusCodes.Status404NotFound,
                ErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorCodes.Invalid => StatusCodes.Status400BadRequest,
                ErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
                ErrorCodes.UserCreationFailed => StatusCodes.Status400BadRequest,
                ErrorCodes.RoleAssignmentFailed => StatusCodes.Status500InternalServerError,
            };

            var problemDetails = new ProblemDetails
            {
                Status = code,
                Title = ReasonPhrases.GetReasonPhrase(code),
            };
            problemDetails.Extensions["errors"] = error.ValidationResult.Errors;

            return problemDetails;
        }
    }
}
