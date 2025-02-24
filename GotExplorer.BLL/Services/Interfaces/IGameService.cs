using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IGameService
    {
        Task<ValidationWithEntityModel<GameResultDTO>> CompleteGameAsync(int gameId, int userId);
        Task<ValidationWithEntityModel<NewGameDTO>> StartGameAsync(string userId);
    }
}