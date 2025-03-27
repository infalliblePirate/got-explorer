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
    public abstract class BaseGameService : IGameService
    {
        protected readonly AppDbContext _appDbContext;
        protected readonly UserManager<User> _userManager;
        protected readonly IMapper _mapper;

        public BaseGameService(
            AppDbContext appDbContext,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public virtual async Task<ValidationWithEntityModel<GameResultDTO>> CompleteGameAsync(int gameId, int userId)
        {
            var game = await _appDbContext.Games
                .Include(g => g.Levels)
                .FirstOrDefaultAsync(g => g.Id == gameId && g.UserId == userId);

            if (game == null)
            {
                return new ValidationWithEntityModel<GameResultDTO>(
                    new ValidationFailure(nameof(gameId), ErrorMessages.GameServiceGameNotFound) { ErrorCode = ErrorCodes.NotFound }
                );
            }

            if (game.UserId != userId)
            {
                return new ValidationWithEntityModel<GameResultDTO>(
                    new ValidationFailure(nameof(userId), ErrorMessages.IncorrectUserId) { ErrorCode = ErrorCodes.Unauthorized }
                );
            }

            if (game.EndTime != null)
            {
                return new ValidationWithEntityModel<GameResultDTO>(
                    new ValidationFailure(nameof(gameId), ErrorMessages.GameAlreadyCompleted) { ErrorCode = ErrorCodes.GameAlreadyCompleted }
                );
            }

            game.EndTime = DateTime.UtcNow;

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                _appDbContext.Games.Update(game);
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ValidationWithEntityModel<GameResultDTO>(
                   new ValidationFailure(nameof(gameId), ErrorMessages.GameServiceFailedToCompleteGame) { ErrorCode = ErrorCodes.GameCompletionFailed }
                );
            }

            var gameResult = _mapper.Map<GameResultDTO>(game);
            gameResult.Score = _appDbContext.GameLevels
                .Where(gl => gl.GameId == game.Id)
                .Sum(gl => gl.Score) ?? 0;

            return new ValidationWithEntityModel<GameResultDTO>(gameResult);
        }

        public virtual async Task<ValidationWithEntityModel<NewGameDTO>> StartGameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new ValidationWithEntityModel<NewGameDTO>(
                    new ValidationFailure(nameof(userId), ErrorMessages.IncorrectUserId, userId) { ErrorCode = ErrorCodes.Unauthorized }
                );
            }

            return await StartGameTransactionAsync(user);
        }

        protected async Task<ValidationWithEntityModel<NewGameDTO>> StartGameTransactionAsync(User user)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            var newGameDto = new NewGameDTO();
            var game = await GetGameAsync(user);
            newGameDto.LevelIds = game.Levels.Select(e => e.Id);

            try
            {
                _appDbContext.Attach(user);
                _appDbContext.AttachRange(game.Levels);

                await _appDbContext.Games.AddAsync(game);
                await _appDbContext.SaveChangesAsync();
                newGameDto.GameId = game.Id;
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new ValidationWithEntityModel<NewGameDTO>(
                    [
                        new ValidationFailure()
                        {
                            ErrorMessage = ErrorMessages.FailedToStartTheGame,
                            ErrorCode = ErrorCodes.GameStartFailed
                        }
                    ]);
            }

            return new ValidationWithEntityModel<NewGameDTO>(newGameDto);
        }

        protected abstract Task<Game> GetGameAsync(User user);
    }
}
