using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Options;
using GotExplorer.BLL.Services.Interfaces;
using GotExplorer.BLL.Services.Results;
using GotExplorer.DAL;
using GotExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GotExplorer.BLL.Services
{
    public class DemoGameService : IDemoGameService
    {
        private readonly AppDbContext _appDbContext;
        private readonly GameOptions _gameOptions;
        public DemoGameService(AppDbContext appDbContext, IOptions<GameOptions> gameOptions) 
        {
            _appDbContext = appDbContext;
            _gameOptions = gameOptions.Value;
        }

        public async Task<ValidationWithEntityModel<NewDemoGameDTO>> StartGameAsync()
        {
            var newGame = new NewDemoGameDTO();
            var levels = await _appDbContext.Levels.Take(_gameOptions.LevelsPerDemoGame).ToListAsync();
            newGame.LevelIds = levels.Select(e => e.Id);
            return new ValidationWithEntityModel<NewDemoGameDTO>(newGame);
        }
    }
}
