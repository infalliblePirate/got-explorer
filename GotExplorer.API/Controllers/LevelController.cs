using GotExplorer.API.Extensions;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace GotExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService _levelService;

        public LevelController(ILevelService levelService) 
        { 
            _levelService = levelService;
        }

        /// <summary>
        /// Retrieves an level by its id
        /// </summary>
        /// <response code="200">The level was successfully updated.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="404">Level not found.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LevelDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> GetLevel([FromRoute] int id)
        {
            var result = await _levelService.GetLevelAsync(id);
            if (!result.IsValid)
            {
                return result.ToActionResult<LevelDTO>();
            }
            result.Entity.Models = result.Entity.Models.Select(model =>
            {
                model.Path = Url.Action(nameof(Model3DController.GetModel3dById), "Model3d", new { id = model.Id }, Request.Scheme);
                return model;
            });
            return result.ToActionResult<LevelDTO>();
        }

        /// <summary>
        /// Create an level. Require authorization and an admin account.
        /// </summary>
        /// <param name="createLevelDTO">The data to create the level.</param>
        /// <response code="200">The level was successfully updated.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Level not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 405)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateLevel([FromBody] CreateLevelDTO createLevelDTO)
        {
            var result = await _levelService.CreateLevelAsync(createLevelDTO);
            return result.ToActionResult();
        }

        /// <summary>
        /// Update an level. Require authorization and an admin account.
        /// </summary>
        /// <param name="id">Level id.</param>
        /// <param name="updateLevelDTO">The data to update the level.</param>
        /// <response code="200">The level was successfully updated.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Level not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 405)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateLevel([FromRoute] int id, [FromBody] UpdateLevelDTO updateLevelDTO)
        {
            var result = await _levelService.UpdateLevelAsync(id, updateLevelDTO);
            return result.ToActionResult();
        }

        /// <summary>
        /// Delete an level. Require authorization and an admin account.
        /// </summary>
        /// <param name="id">Level id.</param>
        /// <response code="200">The level was successfully deleted.</response>
        /// <response code="400">The request data is invalid or incomplete.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Level not found.</response>
        /// <response code="405">The HTTP method is not allowed for the requested resource.</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 405)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(Roles = "Admin")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteLevel([FromRoute] int id)
        {
            var result = await _levelService.DeleteLevelAsync(id);
            return result.ToActionResult();
        }
    }
}
