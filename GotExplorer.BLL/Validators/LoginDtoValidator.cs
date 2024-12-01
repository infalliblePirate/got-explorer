using FluentValidation;
using GotExplorer.BLL.DTOs;

namespace GotExplorer.BLL.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDTO>
    {
        public LoginDtoValidator() 
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.");
           
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
