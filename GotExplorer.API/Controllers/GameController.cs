using GotExplorer.API.Extensions;
using GotExplorer.BLL.DTOs;
using GotExplorer.BLL.Services;
using GotExplorer.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using GotExplorer.DAL.Entities;
using GotExplorer.BLL.Services.Results;
namespace GotExplorer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _standardGameService;
        private readonly IGameService _dailyGameService;
        private readonly IGameLevelService _gameLevelService;
        private readonly IDemoGameService _demoGameService;

        public GameController(
            [FromKeyedServices("standard")] IGameService standardGameService,
            [FromKeyedServices("daily")] IGameService dailyGameService,
            IDemoGameService demoGameService,
            IGameLevelService gameLevelService)
        {
            _standardGameService = standardGameService;
            _dailyGameService = dailyGameService;
            _demoGameService = demoGameService;
            _gameLevelService = gameLevelService;
        }


        /// <summary>
        /// Start a new standard game. Require Authorization.
        /// </summary>
        /// <response code="200">A new game was successfully created.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>        
        [HttpPost("start")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(NewGameDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> StartGame()
        {
            var userId = User.GetClaimValue("Id");
            var result = await _standardGameService.StartGameAsync(userId);
            return result.ToActionResult<NewGameDTO>();
        }

        /// <summary>
        /// Complete the standard game. Require Authorization.
        /// </summary>
        /// <response code="200">The game was successfully completed.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Game not found.</response>
        /// <response code="409">The game has already been completed and cannot be finished again.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>      
        [HttpPut("{id:int}/complete")]
        [ProducesResponseType(typeof(GameResultDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 409)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CompleteGame([FromRoute] int id)
        {
            var userId = User.GetClaimValue("Id");
            var result = await _standardGameService.CompleteGameAsync(id, int.Parse(userId));
            return result.ToActionResult<GameResultDTO>();
        }

        /// <summary>
        /// Calculate the score for a specific game and level.
        /// </summary>
        /// <response code="200">Score calculated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Game or level not found.</response>
        /// <response code="409">The game has already been completed and the level cannot be finished again.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>
        [HttpPut("{id:int}/calculateScore")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(UpdateGameLevelDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 409)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> CalculateScore([FromRoute] int id, [FromBody] CalculateScoreDTO calculateScoreDTO)
        {
            calculateScoreDTO.UserId = User.GetClaimValue("Id");
            calculateScoreDTO.GameId = id;
            var result = await _gameLevelService.CalculateScoreAsync(calculateScoreDTO);
            return result.ToActionResult<UpdateGameLevelDTO>();
        }

        /// <summary>
        /// Calculate the score for a specific level.
        /// </summary>
        /// <response code="200">Score calculated successfully.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="404">Level not found.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>
        [HttpPut("calculateLevelScore")]
        [ProducesResponseType(typeof(LevelScoreDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> CalculateScore([FromBody] CalculateLevelScoreDTO calculateScoreDTO)
        {
            var result = await _gameLevelService.CalculateScoreAsync(calculateScoreDTO);
            return result.ToActionResult<LevelScoreDTO>();
        }

        /// <summary>
        /// Start a new daily game. Require Authorization.
        /// </summary>
        /// <response code="200">A new game was successfully created.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="429">User has reached daily limit of games</response>
        /// <response code="500">An unexpected error occurred on the server.</response>       
        [HttpPost("start/daily")]
        [ProducesResponseType(typeof(NewGameDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 429)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> StartDailyGame()
        {
            var userId = User.GetClaimValue("Id");
            var result = await _dailyGameService.StartGameAsync(userId);
            return result.ToActionResult<NewGameDTO>();
        }

        /// <summary>
        /// Complete the daily game. Require Authorization.
        /// </summary>
        /// <response code="200">The game was successfully completed.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="401">Authentication failed due to invalid JWT.</response>
        /// <response code="403">User does not have sufficient permissions.</response>
        /// <response code="404">Game not found.</response>
        /// <response code="409">The game has already been completed and cannot be finished again.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>   
        [HttpPut("{id:int}/complete/daily")]
        [ProducesResponseType(typeof(GameResultDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 400)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 403)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 409)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CompleteDailyGame([FromRoute] int id)
        {
            var userId = User.GetClaimValue("Id");
            var result = await _dailyGameService.CompleteGameAsync(id, int.Parse(userId));
            return result.ToActionResult<GameResultDTO>();
        }


        /// <summary>
        /// Start a new daily game.
        /// </summary>
        /// <response code="200">A new game was successfully created.</response>
        /// <response code="400">Invalid request data.</response>
        /// <response code="500">An unexpected error occurred on the server.</response>     
        [AllowAnonymous]
        [HttpPost("start/demo")]
        [ProducesResponseType(typeof(NewDemoGameDTO), 200)]
        [ProducesResponseType(typeof(ValidationResult), 401)]
        [ProducesResponseType(typeof(ValidationResult), 404)]
        [ProducesResponseType(typeof(ValidationResult), 500)]
        public async Task<IActionResult> StartDemoGame()
        {
            var result = await _demoGameService.StartGameAsync();
            return result.ToActionResult<NewDemoGameDTO>();
        }
    }
}
