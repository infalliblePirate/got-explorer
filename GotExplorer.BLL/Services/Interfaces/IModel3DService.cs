using FluentValidation.Results;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IModel3DService
    {
        Task<ValidationWithEntityModel<Model3dDTO>> GetModel3dAsync(int id);
        Task<ValidationResult> UploadModel3dAsync(UploadModel3dDTO uploadModel3dDTO);
        Task<ValidationResult> UpdateModel3dAsync(int id, UploadModel3dDTO uploadModel3dDTO);
        Task<ValidationResult> DeleteModel3dAsync(int id);
    }
}
