using CrashGameService.Models;
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

        [HttpPost("bet")]
        public async ValueTask<IActionResult> Bet(BetRequest request)
        {
            return Ok(await _gameService.Bet(request));
        }

        [HttpPost("cashOut")]
        public async ValueTask<IActionResult> CashOut(CashOutRequest request)
        {
            return Ok(await _gameService.CashOut(request));
        }

        [HttpGet("start")]
        public async ValueTask<IActionResult> StartGame()
        {
            await _gameService.StartGame();
            return Ok("Game started");
        }
    }
}
