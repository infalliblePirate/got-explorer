using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.DAL;
using GotExplorer.DAL.Entities;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GotExplorer.BLL.Options;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace GotExplorer.BLL.Services
{
    public class DailyGameService : BaseGameService
    {
        public DailyGameService(
            AppDbContext appDbContext,
            UserManager<User> userManager,
            IMapper mapper) : base(appDbContext,userManager,mapper) {}


        public override async Task<ValidationWithEntityModel<NewGameDTO>> StartGameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ValidationWithEntityModel<NewGameDTO>(
                    new ValidationFailure(nameof(userId), ErrorMessages.IncorrectUserId, userId) { ErrorCode = ErrorCodes.Unauthorized }
                );
            }

            DateTime today = DateTime.Today.ToUniversalTime();
            DateTime tomorrow = today.AddDays(1).ToUniversalTime();

            var playedGameToday = await _appDbContext.Games.Where(g => g.GameType == GameType.Daily && g.StartTime >= today && g.EndTime <= tomorrow).AnyAsync();

            if (playedGameToday) {
                return new ValidationWithEntityModel<NewGameDTO>(
                    new ValidationFailure(userId, ErrorMessages.DailyGamePlayLimitExceeded) { ErrorCode = ErrorCodes.PlayLimitExceeded }
                );
            }

            return await StartGameTransactionAsync(user);
        }

        protected override async Task<Game> GetGameAsync(User user)
        {
            var levelsCount = await _appDbContext.Levels.CountAsync();
            var currentDay = DateTime.Now.DayOfYear;
            var levelIndex = currentDay % levelsCount;

            return new Game()
            {
                User = user,
                StartTime = DateTime.UtcNow,
                GameType = GameType.Daily,
                Levels = await _appDbContext.Levels.Skip(levelIndex).Take(1).ToListAsync(),
            };
        }
    }
}
