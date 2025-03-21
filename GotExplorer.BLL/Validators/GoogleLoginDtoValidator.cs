using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class GoogleLoginDtoValidator : AbstractValidator<GoogleLoginDTO>
    {
        public GoogleLoginDtoValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.GoogleAuthCredentialRequired);
        }
    }
}
