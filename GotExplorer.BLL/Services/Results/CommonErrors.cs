using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Results
{
    public static class CommonErrors
    {
        public static Error NotFoundError(ValidationResult result) => new(ErrorCodes.NotFound, result);
        public static Error UnauthorizedError(ValidationResult result) => new(ErrorCodes.Unauthorized, result);
        public static Error InvalidError(ValidationResult result) => new(ErrorCodes.Invalid, result);
        public static Error ForbiddenError(ValidationResult result) => new(ErrorCodes.Forbidden, result);
        public static Error UserCreationFailedError(ValidationResult result) => new(ErrorCodes.UserCreationFailed, result);
        public static Error RoleAssignmentFailedError(ValidationResult result) => new (ErrorCodes.RoleAssignmentFailed, result);
    }
}
