using FluentValidation;
using GotExplorer.BLL.DTOs;
namespace GotExplorer.BLL.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDtoValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id must be not null");
            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email address");
            RuleFor(x => x.Username)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Username))
                .WithMessage("Username cannot be empty");
            RuleFor(x => x.ImageId)
                .Must(x => x > 0)
                .When(x => x.ImageId != null)
                .WithMessage("ImageId must be null or greater than 0");
        }
    }
}
