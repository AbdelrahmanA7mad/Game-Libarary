using GameApi.Dtos;
using GameApi.Models;
using GameApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameApi.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("GetGames")]
        public async Task<ActionResult> GetGames()
        {
            try
            {
                var games = await _gameService.GetAllGamesAsync();

                if (games == null || !games.Any())
                {
                    return NotFound("No games found.");
                }

                var gameDtos = games.Select(game => new Gamedto
                {
                    id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    ImageUrl = game.ImageUrl,
                    VideoUrl = game.VideoUrl,
                    Categoryid = game.Categoryid,
                    CategoryName = game.category?.Name, 
                    GameDevice = game.GameDevices?.Select(gd => gd.Name).ToList(),
                     GameDeviceIds= game.GameDevices?.Select(gd => gd.Id).ToList() 
                }).ToList();

                return Ok(gameDtos); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetGameById/{id}")]
        public async Task<ActionResult> GetGameById(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            return Ok(game);
        }

        [HttpPost("CreateGame")]
        public async Task<IActionResult> CreateGame([FromBody] AddGameDto dto)
        {
            try
            {
                var game = await _gameService.CreateGameAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteGame/{id}")]
        public async Task<ActionResult> DeleteGame(int id)
        {
            var result = await _gameService.DeleteGameAsync(id);

            if (!result)
            {
                return NotFound();  
            }

            return NoContent(); 
        }


        [HttpPut("UpdateGame/{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameDto updateGameDto)
        {
            try
            {
                var result = await _gameService.UpdateGameAsync(id, updateGameDto);

                if (!result)
                    return NotFound($"Game with ID {id} not found.");

                return Ok("Game updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
