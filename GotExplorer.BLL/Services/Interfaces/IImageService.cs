using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Results;
using System.Collections;

namespace GotExplorer.BLL.Services.Interfaces
{
    public interface IImageService
    {
        Task<ServiceResult<ImageDTO>> GetImageAsync(int id);
        Task<ServiceResult<IEnumerable<ImageDTO>>> GetAllImagesAsync();
        Task<ServiceResult> UploadImageAsync(IFormFile image);
        Task<ServiceResult> UpdateImageAsync(int id, IFormFile imageFile);
        Task<ServiceResult> DeleteImageAsync(int id);
    }
}
