using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class CompleteGameDTOValidator : AbstractValidator<CompleteGameDTO>
    {
        public CompleteGameDTOValidator()
        {
            RuleFor(x => x.GameId).GreaterThan(0).WithMessage("Game ID must be a positive number.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID must be a positive number.");
        }
    }
}