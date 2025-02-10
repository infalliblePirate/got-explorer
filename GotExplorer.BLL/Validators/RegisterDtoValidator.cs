using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using Microsoft.AspNetCore.Rewrite;
namespace GotExplorer.BLL.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDtoValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.EmailRequired)
                .EmailAddress()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidEmail);
            RuleFor(x => x.Password)
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
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.UsernameRequired);
        }
    }
}
