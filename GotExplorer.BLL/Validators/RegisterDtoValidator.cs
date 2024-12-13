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
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Email must be a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[0-9]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password must contain at least one digit.")
                .Matches(@"[a-z]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[^a-zA-Z0-9]")
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Username is required.");
        }
    }
}
