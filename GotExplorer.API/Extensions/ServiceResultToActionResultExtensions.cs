using GotExplorer.BLL.Services.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System;
using FluentValidation.Results;

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
            return GetActionResult(serviceResult.ValidationResult);
        }
        public static IActionResult ToActionResult(this ServiceResult serviceResult)
        {
            if (serviceResult.IsSuccess)
            {
                return new OkResult();
            }
            return GetActionResult(serviceResult.ValidationResult);
        }
        private static IActionResult GetActionResult(ValidationResult? validationResult)
        {
            var result = new ObjectResult(validationResult);

            var countUniqueErrors = validationResult.Errors
                .Select(e => e.ErrorCode)
                .Distinct().Count();

            if (countUniqueErrors > 1)
            {
                result.StatusCode = StatusCodes.Status422UnprocessableEntity;
                return result;
            }

            result.StatusCode = validationResult.Errors[0].ErrorCode switch
            {
                ErrorCodes.NotFound => StatusCodes.Status404NotFound,
                ErrorCodes.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorCodes.Invalid => StatusCodes.Status400BadRequest,
                ErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
                ErrorCodes.UserCreationFailed => StatusCodes.Status400BadRequest,
                ErrorCodes.RoleAssignmentFailed => StatusCodes.Status500InternalServerError,
                ErrorCodes.UserUpdateFailed => StatusCodes.Status400BadRequest,
                ErrorCodes.UserPasswordUpdateFailed => StatusCodes.Status400BadRequest,
                ErrorCodes.UserResetPasswordFailed => StatusCodes.Status400BadRequest,
                ErrorCodes.UserDeletionFailed => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError,
            };
            return result;
        }
    }
}
