using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IGameLevelService
    {
        Task<ValidationWithEntityModel<UpdateGameLevelDTO>> CalculateScoreAsync(CalculateScoreDTO calculateScoreDTO);
    }
}