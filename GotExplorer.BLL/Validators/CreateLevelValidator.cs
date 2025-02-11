using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class CreateLevelValidator : AbstractValidator<CreateLevelDTO>
    {
        public CreateLevelValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Name is required.");
            RuleFor(x => x.X)
                .NotNull()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Position x is required");
            RuleFor(x => x.Y)
                .NotNull()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Position y is required");
        }
    }
}
