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
using FluentValidation.Results;
using System.Text.RegularExpressions;

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
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VirtualFileResult), 200)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(VirtualFileResult), 206)]
        [ProducesResponseType(typeof(VirtualFileResult), 416)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> GetImageById(string id)
        {
            var a = Guid.Parse(id);
            var result = await _imageService.GetImageAsync(a);
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
            Response.Headers.ContentDisposition = new ContentDisposition() { Inline = true, FileName = image.Name.RemoveNonAsciiChars() }.ToString();
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
        [ProducesResponseType(typeof(ValidationResult), 500)]
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
        /// <param name="image">Image file.</param>
        /// <response code="200">The image was successfully uploaded.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 405)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDTO image)
        {
            var result = await _imageService.UploadImageAsync(image);
            return result.ToActionResult();
        }

        /// <summary>
        /// Update an image. Require authorization and an admin account.
        /// </summary>
        /// <param name="id">Image id.</param>
        /// <param name="image">Image file.</param>
        /// <response code="200">The image was successfully updated.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Image not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 405)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateImage([FromRoute] Guid id, [FromForm] UploadImageDTO image)
        {
            var result = await _imageService.UpdateImageAsync(id, image);
            return result.ToActionResult();
        }

        /// <summary>
        /// Delete an image. Require authorization and an admin account.
        /// </summary>
        /// <param name="id">Image id.</param>
        /// <response code="200">The image was successfully deleted.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Image not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 405)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var result = await _imageService.DeleteImageAsync(id);
            return result.ToActionResult();
        }
    }
}
