using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.Collections;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IImageService
    {
        Task<ServiceResult<ImageDTO>> GetImageAsync(Guid id);
        Task<ServiceResult<IEnumerable<ImageDTO>>> GetAllImagesAsync();
        Task<ServiceResult> UploadImageAsync(UploadImageDTO uploadImageDTO);
        Task<ServiceResult> UpdateImageAsync(Guid id, UploadImageDTO uploadImageDTO);
        Task<ServiceResult> DeleteImageAsync(Guid id);
    }
}
