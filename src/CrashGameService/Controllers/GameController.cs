using CrashGameService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrashGameService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("bet")]
        public async Task<IActionResult> Bet()
        {
            return Ok(new { Message = "Ok"});
        }

        [HttpPost("cashOut")]
        public async Task<IActionResult> CashOut()
        {
            return Ok();
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame()
        {
            return Ok();
        }
    }
}
