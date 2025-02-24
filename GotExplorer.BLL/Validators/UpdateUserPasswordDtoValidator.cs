using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDTO>
    {
        public UpdateUserPasswordDtoValidator() 
        {
            RuleFor(x => x.Id)         
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.IdRequired);
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordRequired)
                .MinimumLength(8)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordMinLength)
                .Matches(@"[0-9]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordMustContainDigit)
                .Matches(@"[a-z]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordMustContainLowercase)
                .Matches(@"[A-Z]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordMustContainUppercase)
                .Matches(@"[^a-zA-Z0-9]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordMustContainSpecial);
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordRequired);
        }
    }
}
