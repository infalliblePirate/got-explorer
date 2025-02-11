using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Results;
using Microsoft.Extensions.Options;

namespace GotExplorer.BLL.Validators
{
    public class CalculateScoreDTOValidator : AbstractValidator<CalculateScoreDTO>
    {
        private readonly GameOptions _gameOptions;

        public CalculateScoreDTOValidator(IOptions<GameOptions> gameOptions)
        {
            _gameOptions = gameOptions.Value;

            RuleFor(x => x.UserId)
                .Must(x => int.TryParse(x, out var val) && val > 0)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.GameServiceGameNotFound);

            RuleFor(x => x.GameId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.GameServiceGameNotFound);

            RuleFor(x => x.LevelId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.LevelServiceLevelNotFound);

            RuleFor(x => x.X)
                .InclusiveBetween(0, _gameOptions.MaxX)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(string.Format(ErrorMessages.GameLevelServiceFailedToSaveScore, 0, _gameOptions.MaxX));

            RuleFor(x => x.Y)
                .InclusiveBetween(0, _gameOptions.MaxY)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(string.Format(ErrorMessages.GameLevelServiceFailedToSaveScore, 0, _gameOptions.MaxY));
        }
    }
}
