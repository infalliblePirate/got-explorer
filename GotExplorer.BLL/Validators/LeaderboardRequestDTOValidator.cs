using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class LeaderboardRequestDTOValidator : AbstractValidator<LeaderboardRequestDTO>
    {
        public LeaderboardRequestDTOValidator()
        {
            RuleFor(x => x.Limit)
                .GreaterThanOrEqualTo(1)
                .When(x => x.Limit.HasValue)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidLimit);

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidLeaderboardSortBy);

            RuleFor(x => x.OrderBy)
                .IsInEnum()
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.InvalidLeaderboardOrderBy);
        }
    }
}