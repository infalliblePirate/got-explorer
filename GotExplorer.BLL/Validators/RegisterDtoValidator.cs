using FluentValidation;
using GotExplorer.BLL.DTOs;
using Microsoft.AspNetCore.Rewrite;
namespace GotExplorer.BLL.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDtoValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[0-9]")
                .WithMessage("Password must contain at least one digit.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character.");
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.");
        }
    }
}
