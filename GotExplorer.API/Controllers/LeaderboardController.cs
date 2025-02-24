using GotExplorer.API.Extensions;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace GotExplorer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        /// <summary>
        /// Retrieves the leaderboard, with optional query parameters for limiting, sorting and ordering the records. By default, it orders by score in descending order.
        /// </summary>
        /// <param name="requestDTO">The DTO containing query parameters to fetch leaderboard data.</param>
        /// <response code="200">The leaderboard data was successfully retrieved.</response>
        /// <response code="400">Invalid request data (e.g., invalid limit value).</response>
        /// <response code="500">An unexpected error occurred on the server.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LeaderboardRecordDTO>),200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> GetLeaderboard([FromQuery] LeaderboardRequestDTO requestDTO)
        {
            var result = await _leaderboardService.GetLeaderboardAsync(requestDTO);
            return result.ToActionResult();
        }

        /// <summary>
        /// Retrieves the leaderboard data for a single player, including the player's rank, with optional query parameters for sorting and ordering. 
        /// By default, it orders by score in descending order.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="queryParams">The DTO containing query parameters to fetch data.</param>
        /// <response code="200">The leaderboard data was successfully retrieved.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>
        [HttpGet("user/{id:int}")]
        [ProducesResponseType(typeof(LeaderboardUserDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> GetUserLeaderboard([FromRoute] int id, [FromQuery] LeaderboardUserRequestDTO queryParams)
        {
            queryParams.UserId = id;
            var result = await _leaderboardService.GetUserLeaderboardAsync(queryParams);
            return result.ToActionResult();
        }
    }
}