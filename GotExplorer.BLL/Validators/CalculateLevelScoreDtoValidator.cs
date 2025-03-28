using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Results;
using Microsoft.Extensions.Options;

namespace GotExplorer.BLL.Validators
{
    public class CalculateLevelScoreDtoValidator : AbstractValidator<CalculateLevelScoreDTO>
    {
        private readonly GameOptions _gameOptions;

        public CalculateLevelScoreDtoValidator(IOptions<GameOptions> gameOptions)
        {
            _gameOptions = gameOptions.Value;

            RuleFor(x => x.LevelId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.LevelServiceLevelNotFound);

            RuleFor(x => x.X)
                .InclusiveBetween(0, _gameOptions.MaxX)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(string.Format(ErrorMessages.ValueMustBeBetween, 0, _gameOptions.MaxX));

            RuleFor(x => x.Y)
                .InclusiveBetween(0, _gameOptions.MaxY)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(string.Format(ErrorMessages.ValueMustBeBetween, 0, _gameOptions.MaxY));
        }
    }
}
