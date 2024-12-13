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
                .WithMessage("Id is required.");
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Email must be a valid email address.");
            RuleFor(x => x.Username)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Username))
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Username cannot be empty.");
            RuleFor(x => x.ImageId)
                .NotEmpty()
                .When(x => x.ImageId != null)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("ImageId is required.");
        }
    }
}
