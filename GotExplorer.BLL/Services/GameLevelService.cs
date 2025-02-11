using AutoMapper;
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


namespace GotExplorer.BLL.Services
{
    public class GameLevelService : IGameLevelService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IValidator<CalculateScoreDTO> _calculateScoreValidator;
        private readonly GameOptions _gameOptions;
        private readonly IMapper _mapper;

        public GameLevelService(IOptions<GameOptions> gameOptions, AppDbContext appDbContext, UserManager<User> userManager, IValidator<CalculateScoreDTO> calculateScoreValidator, IMapper mapper)
        {
            _gameOptions = gameOptions.Value;
            _appDbContext = appDbContext;
            _calculateScoreValidator = calculateScoreValidator;
            _userManager = userManager;
            _mapper = mapper;
        }

         public async Task<ValidationWithEntityModel<UpdateGameLevelDTO>> CalculateScoreAsync(CalculateScoreDTO calculateScoreDTO)
        {
            var validationResult = await _calculateScoreValidator.ValidateAsync(calculateScoreDTO);
            if (!validationResult.IsValid)
            {
                return new ValidationWithEntityModel<UpdateGameLevelDTO>(validationResult);
            }

            var user = await _userManager.FindByIdAsync(calculateScoreDTO.UserId);
            if (user == null)
            {
                return ValidationWithEntityModel<UpdateGameLevelDTO>.GenerateValidationFailure<UpdateGameLevelDTO>(
                    nameof(calculateScoreDTO.UserId), ErrorMessages.IncorrectUserId, calculateScoreDTO.UserId, ErrorCodes.Unauthorized);
            }

            var game = await _appDbContext.Games.FindAsync(calculateScoreDTO.GameId);
            if (game == null)
            {
                return ValidationWithEntityModel<UpdateGameLevelDTO>.GenerateValidationFailure<UpdateGameLevelDTO>(
                    nameof(calculateScoreDTO.GameId), ErrorMessages.GameServiceGameNotFound, calculateScoreDTO.GameId, ErrorCodes.NotFound);
            }

            if (game.UserId != user.Id)
            {
                return ValidationWithEntityModel<UpdateGameLevelDTO>.GenerateValidationFailure<UpdateGameLevelDTO>(
                    nameof(calculateScoreDTO.GameId), ErrorMessages.IncorrectUserId, calculateScoreDTO.GameId, ErrorCodes.Unauthorized);
            }

            if (game.EndTime != null)
            {
                return ValidationWithEntityModel<UpdateGameLevelDTO>.GenerateValidationFailure<UpdateGameLevelDTO>(
                    nameof(calculateScoreDTO.GameId), ErrorMessages.GameAlreadyCompleted, calculateScoreDTO.GameId, ErrorCodes.GameAlreadyCompleted);
            }

            var gameLevel = await _appDbContext.GameLevels
                .Where(gl => gl.GameId == calculateScoreDTO.GameId && gl.LevelId == calculateScoreDTO.LevelId)
                .FirstOrDefaultAsync();
            
            if (gameLevel == null)
            {
                return ValidationWithEntityModel<UpdateGameLevelDTO>.GenerateValidationFailure<UpdateGameLevelDTO>(
                    nameof(calculateScoreDTO.LevelId), ErrorMessages.GameLevelServiceGameLevelNotFound, calculateScoreDTO.LevelId, ErrorCodes.NotFound);
            }

            var requestedLevel = await _appDbContext.Levels.FindAsync(calculateScoreDTO.LevelId);
            if (requestedLevel == null)
            {
                return ValidationWithEntityModel<UpdateGameLevelDTO>.GenerateValidationFailure<UpdateGameLevelDTO>(
                    nameof(calculateScoreDTO.LevelId), ErrorMessages.LevelServiceLevelNotFound, calculateScoreDTO.LevelId, ErrorCodes.NotFound);
            }

            int score = CalculateLevelScore(requestedLevel.X, requestedLevel.Y, calculateScoreDTO.X, calculateScoreDTO.Y);

            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            try
            {
                gameLevel.Score = score;
                _appDbContext.GameLevels.Update(gameLevel);
                await _appDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var updatedGameLevelDTO = _mapper.Map<UpdateGameLevelDTO>(gameLevel);
                return new ValidationWithEntityModel<UpdateGameLevelDTO>(updatedGameLevelDTO);
            }
            catch
            {
                await transaction.RollbackAsync();

                return new ValidationWithEntityModel<UpdateGameLevelDTO>(
                    new ValidationFailure(nameof(calculateScoreDTO.LevelId), ErrorMessages.GameLevelServiceFailedToSaveScore)
                    {
                        ErrorCode = ErrorCodes.GameLevelUpdateFailed
                    });
            }
        }

        private double CalculateDistance(double correctX, double correctY, double chosedX, double chosedY)
        {
            return Math.Sqrt(Math.Pow(chosedX - correctX, 2) + Math.Pow(chosedY - correctY, 2));
        }

        private int CalculateLevelScore(double correctX, double correctY, double chosedX, double chosedY)
        {
            double distance = CalculateDistance(correctX, correctY, chosedX, chosedY);

            if (distance <= _gameOptions.ProximityRadius)
                return _gameOptions.ScoreWithinRadius;

            double excessDistance = distance - _gameOptions.ProximityRadius;
            double penalty = excessDistance / _gameOptions.PenaltyStep * _gameOptions.PenaltyPerStep;
            return Math.Max(0, _gameOptions.ScoreWithinRadius - (int)Math.Round(penalty));
        }

    }
}