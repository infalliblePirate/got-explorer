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
    public class Model3DController : ControllerBase
    {
        private readonly IModel3DService _model3DService;
        public Model3DController(IModel3DService model3DService)
        {
            _model3DService = model3DService;
        }

        /// <summary>
        ///  Retrieves an model file by its id.
        /// </summary>
        /// <param name="id">Model id.</param>
        /// <response code="200">The model was successfully retrieved.</response>
        /// <response code="206">Partial content.</response>
        /// <response code="404">Model not found.</response>
        /// <response code="416">Range is not satisfiable.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(VirtualFileResult), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(VirtualFileResult), 206)]
        [ProducesResponseType(typeof(VirtualFileResult), 416)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetModel3dById(int id)
        {
            var result = await _model3DService.GetModel3dAsync(id);
            if (!result.IsValid)
            {
                return result.ToActionResult<Model3dDTO>();
            }
            var model = result.Entity;
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            var fileExtension = Path.GetExtension(model.Path).ToLowerInvariant();

            if (!contentTypeProvider.TryGetContentType(model.Path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            Response.Headers.ContentType = contentType;
            Response.Headers.XContentTypeOptions = "nosniff";
            return File(model.Path, contentType,model.Name);
        }

        /// <summary>
        /// Upload an model. Require authorization and an admin account.
        /// </summary>
        /// <response code="200">The model was successfully uploaded.</response>
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
        public async Task<IActionResult> UploadModel([FromForm] UploadModel3dDTO model)
        {
            var result = await _model3DService.UploadModel3dAsync(model);
            return result.ToActionResult();
        }

        /// <summary>
        /// Update an model. Require authorization and an admin account.
        /// </summary>
        /// <response code="200">The model was successfully updated.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">model not found.</response>
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
        public async Task<IActionResult> UpdateModel3d([FromRoute] int id, [FromForm] UploadModel3dDTO model)
        {
            var result = await _model3DService.UpdateModel3dAsync(id, model);
            return result.ToActionResult();
        }

        /// <summary>
        /// Delete an model. Require authorization and an admin account.
        /// </summary>
        /// <response code="200">The model was successfully deleted.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid. JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">model not found.</response>
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
        public async Task<IActionResult> DeleteModel3d(int id)
        {
            var result = await _model3DService.DeleteModel3dAsync(id);
            return result.ToActionResult();
        }
    }
}
