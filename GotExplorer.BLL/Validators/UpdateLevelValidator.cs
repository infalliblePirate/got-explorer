using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class UpdateLevelValidator : AbstractValidator<UpdateLevelDTO>
    {
        public UpdateLevelValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage("Name is required.");
        }
    }
}
