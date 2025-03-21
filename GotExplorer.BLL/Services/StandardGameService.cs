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
    public class StandardGameService : BaseGameService
    {
        private readonly GameOptions _gameOptions;
        public StandardGameService(
            IOptions<GameOptions> gameOptions, 
            AppDbContext appDbContext, 
            UserManager<User> userManager,
            IMapper mapper) : base(appDbContext,userManager,mapper)
        {
            _gameOptions = gameOptions.Value;
        }

        protected override async Task<Game> GetGameAsync(User user)
        {
            return new Game()
            {
                User = user,
                StartTime = DateTime.UtcNow,
                GameType = GameType.Standard,
                Levels = await _appDbContext.Levels.OrderBy(r => Guid.NewGuid()).Take(_gameOptions.LevelsPerGame).ToListAsync()
            };
        }
    }
}
