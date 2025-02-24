using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface ILevelService
    {
        Task<ValidationWithEntityModel<LevelDTO>> GetLevelAsync(int id);
        Task<ValidationResult> CreateLevelAsync(CreateLevelDTO levelDTO);
        Task<ValidationResult> UpdateLevelAsync(int id, UpdateLevelDTO levelDTO);
        Task<ValidationResult> DeleteLevelAsync(int id);
    }
}
