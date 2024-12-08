using GotExplorer.API.Extensions;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Net.Mime;
using static System.Net.WebRequestMethods;

namespace GotExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        ///  Retrieves an image file by its id.
        /// </summary>
        /// <param name="id">Image id.</param>
        /// <response code="200">The image was successfully retrieved.</response>
        /// <response code="206">Partial content.</response>
        /// <response code="404">Image not found.</response>
        /// <response code="416">Range is not satisfiable.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VirtualFileResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(VirtualFileResult), 206)]
        [ProducesResponseType(typeof(VirtualFileResult), 416)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetImageById(int id)
        {
            var result = await _imageService.GetImageAsync(id);
            if (!result.IsSuccess)
            {
                return result.ToActionResult<ImageDTO>();
            }
            var image = result.ResultObject;
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            var fileExtension = Path.GetExtension(image.Path).ToLowerInvariant();

            if (!contentTypeProvider.TryGetContentType(image.Path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            Response.Headers.ContentDisposition = new ContentDisposition() { Inline = true, FileName = image.Name }.ToString();
            Response.Headers.ContentType = contentType;
            Response.Headers.XContentTypeOptions = "nosniff";
            return File(image.Path, contentType);
        }

        /// <summary>
        ///  Retrieves an array of images.
        /// </summary>
        /// <response code="200">A list of images was successfully retrieved.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<ImageDTO>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetAllImages()
        {
            var result = await _imageService.GetAllImagesAsync();
            if (!result.IsSuccess)
            {
                return result.ToActionResult<IEnumerable<ImageDTO>>();
            }
            var images = result.ResultObject.Select(image =>
            {
                image.Path = Url.Action(nameof(GetImageById), "Image", new { id = image.Id }, Request.Scheme);
                return image;
            }).ToList();
            return Ok(images);
        }

        /// <summary>
        /// Upload an image. Require authorization and an admin account.
        /// </summary>
        /// <response code="200">The image was successfully uploaded.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        [ProducesResponseType(typeof(ProblemDetails), 405)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            var result = await _imageService.UploadImageAsync(image);
            return result.ToActionResult();
        }

        /// <summary>
        /// Update an image. Require authorization and an admin account.
        /// </summary>
        /// <response code="200">The image was successfully updated.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Image not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 405)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateImage([FromRoute] int id, IFormFile image)
        {
            var result = await _imageService.UpdateImageAsync(id, image);
            return result.ToActionResult();
        }

        /// <summary>
        /// Delete an image. Require authorization and an admin account.
        /// </summary>
        /// <response code="200">The image was successfully deleted.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Image not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 403)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 405)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var result = await _imageService.DeleteImageAsync(id);
            return result.ToActionResult();
        }
    }
}
