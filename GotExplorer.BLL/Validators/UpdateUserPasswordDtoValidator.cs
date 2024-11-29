using FluentValidation;
using GotExplorer.BLL.DTOs;

namespace GotExplorer.BLL.Validators
{
    public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDTO>
    {
        public UpdateUserPasswordDtoValidator() 
        {
            RuleFor(x => x.Id)         
                .NotEmpty()
                .WithMessage("Id is required");
            RuleFor(x => x.NewPassword)
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
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
