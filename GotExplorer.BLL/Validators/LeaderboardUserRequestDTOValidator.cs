using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Validators
{
    public class LeaderboardUserRequestDTOValidator : AbstractValidator<LeaderboardUserRequestDTO>
    {
        public LeaderboardUserRequestDTOValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThanOrEqualTo(1)
                .WithErrorCode(ErrorCodes.Invalid)
                .WithMessage(ErrorMessages.IncorrectUserId);

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
