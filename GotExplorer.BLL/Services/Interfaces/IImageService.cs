using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.Collections;
using FluentValidation.Results;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IImageService
    {
        Task<ValidationWithEntityModel<ImageDTO>> GetImageAsync(Guid id);
        Task<ValidationWithEntityModel<IEnumerable<ImageDTO>>> GetAllImagesAsync();
        Task<ValidationResult> UploadImageAsync(UploadImageDTO uploadImageDTO);
        Task<ValidationResult> UpdateImageAsync(Guid id, UploadImageDTO uploadImageDTO);
        Task<ValidationResult> DeleteImageAsync(Guid id);
    }
}
