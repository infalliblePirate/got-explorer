using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
namespace GotExplorer.BLL.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDtoValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.IdRequired);
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidEmail);
            RuleFor(x => x.Username)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Username))
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.UsernameRequired);
            RuleFor(x => x.ImageId)
                .NotEmpty()
                .When(x => x.ImageId != null)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.UpdateUserImageId);
        }
    }
}
