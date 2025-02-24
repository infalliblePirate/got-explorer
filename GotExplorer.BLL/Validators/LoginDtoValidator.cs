using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDTO>
    {
        public LoginDtoValidator() 
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.UsernameRequired);
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.PasswordRequired);
        }
    }
}
