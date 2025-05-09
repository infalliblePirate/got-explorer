using AutoMapper;
using FluentValidation;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.BLL.Validators;
using GotExplorer.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using GotExplorer.DAL;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using GotExplorer.BLL.Extensions;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Globalization;
using System.Xml.Linq;

namespace GotExplorer.BLL.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IValidator<LeaderboardRequestDTO> _leaderboardRequestValidator;
        private readonly IValidator<LeaderboardUserRequestDTO> _leaderboardUserRequestValidator;
        private readonly IMapper _mapper;
        public LeaderboardService(AppDbContext appDbContext, 
            IValidator<LeaderboardRequestDTO> leaderboardRequestValidator, 
            IValidator<LeaderboardUserRequestDTO> leaderboardUserRequestValidator,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _leaderboardRequestValidator = leaderboardRequestValidator;
            _leaderboardUserRequestValidator = leaderboardUserRequestValidator;
            _mapper = mapper;
        }

        public async Task<ValidationWithEntityModel<List<LeaderboardRecordDTO>>> GetLeaderboardAsync(LeaderboardRequestDTO requestDTO)
        {
            var validationResult = await _leaderboardRequestValidator.ValidateAsync(requestDTO);
            if (!validationResult.IsValid )
            {
                return new ValidationWithEntityModel<List<LeaderboardRecordDTO>>(validationResult);
            }
            var gameType = _mapper.Map<GameType>(requestDTO.GameType);


            var leaderboard = await _appDbContext.Games.Include(x => x.User)
                .Where(game => game.EndTime != null && game.GameType == gameType)
                .Where(GetDateFilter(gameType))
                .Select(x => new LeaderboardRecordDTO()
                {
                    UserId = x.UserId,
                    StartTime = x.StartTime,
                    Username = x.User.UserName,
                    EndTime = x.EndTime.Value,
                    Score = _appDbContext.GameLevels
                     .Where(gl => gl.GameId == x.Id)
                     .Sum(gl => gl.Score ?? 0),
                })
               .GroupBy(x => x.UserId)
               .OrderByDynamic(GetSortBy(requestDTO.SortBy), requestDTO.OrderBy)
               .Select(g => g.OrderByDescending(x => x.Score).First())
               .TakeOrDefault(requestDTO.Limit)
               .ToListAsync();

            return new ValidationWithEntityModel<List<LeaderboardRecordDTO>>(leaderboard);
        }

        public async Task<ValidationWithEntityModel<LeaderboardUserDTO>> GetUserLeaderboardAsync(LeaderboardUserRequestDTO requestDTO)
        {
            var validationResult = await _leaderboardUserRequestValidator.ValidateAsync(requestDTO);
            if (!validationResult.IsValid)
            {
                return new ValidationWithEntityModel<LeaderboardUserDTO>(validationResult);
            }

            var gameType = _mapper.Map<GameType>(requestDTO.GameType);

            var userRecord = _appDbContext.Games.Include(x => x.User)
                .Where(game => game.EndTime != null && game.UserId == requestDTO.UserId && game.GameType == gameType)
                .Where(GetDateFilter(gameType))
                .Select(x => new LeaderboardUserDTO()
                {
                    UserId = x.UserId,
                    Username = x.User.UserName,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime.Value,
                    Score = _appDbContext.GameLevels
                     .Where(gl => gl.GameId == x.Id)
                     .Sum(gl => gl.Score ?? 0),
                })
               .GroupBy(x => x.UserId)
               .Select(g => g.OrderByDescending(x => x.Score).First())
               .FirstOrDefault();

            if (userRecord == null)
            {
                return new ValidationWithEntityModel<LeaderboardUserDTO>(
                    new ValidationFailure(nameof(requestDTO.UserId), ErrorMessages.UserServiceUserNotFound) { ErrorCode = ErrorCodes.NotFound }
                );
            }

            var leaderboard = _appDbContext.Games
                .Where(game => game.EndTime != null && game.GameType == gameType)
                .Select(x => new
                {
                    UserId = x.UserId,
                    Score = _appDbContext.GameLevels
                        .Where(gl => gl.GameId == x.Id)
                        .Sum(gl => gl.Score ?? 0),
                    TimeDiff = x.EndTime - x.StartTime
                })
                .GroupBy(x => x.UserId);

            if (requestDTO.SortBy == LeaderboardSortBy.Score)
            {
                var scoreRecords = leaderboard.Select(g => g.Max(s => s.Score));
                if (requestDTO.OrderBy == OrderBy.Desc)
                    userRecord.Rank = await scoreRecords.CountAsync(s => s > userRecord.Score) + 1;    
                else 
                    userRecord.Rank = await scoreRecords.CountAsync(s => s < userRecord.Score) + 1;
            }
            else
            {
                var userTime = userRecord.EndTime - userRecord.StartTime;

                var scoreRecords = leaderboard.Select(g => g.OrderByDescending(s => s.Score).FirstOrDefault().TimeDiff);
                if (requestDTO.OrderBy == OrderBy.Desc)
                    userRecord.Rank = await scoreRecords.CountAsync(s => s > userTime) + 1;
                else
                    userRecord.Rank = await scoreRecords.CountAsync(s => s < userTime) + 1;
            }

            return new ValidationWithEntityModel<LeaderboardUserDTO>(userRecord);
        }

        private Expression<Func<IEnumerable<LeaderboardRecordDTO>, object>> GetSortBy(LeaderboardSortBy sortBy)
        {
            return sortBy switch
            {
                LeaderboardSortBy.Time => x => x.Min(s => s.EndTime - s.StartTime),
                LeaderboardSortBy.Score => x => x.Max(s => s.Score),
            };
        }
        private Expression<Func<Game, bool>> GetDateFilter(GameType gameType)
        {
            if (gameType == GameType.Daily)
            {
                DateTime today = DateTime.Today.ToUniversalTime();
                DateTime tomorrow = today.AddDays(1).ToUniversalTime();
                return g => g.StartTime >= today && g.EndTime <= tomorrow;
            }
            return g => true;
        }
    }
}