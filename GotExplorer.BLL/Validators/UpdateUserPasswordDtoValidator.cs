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
                .WithMessage("Id is required");
            RuleFor(x => x.NewPassword)
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
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Password is required.");
        }
    }
}
