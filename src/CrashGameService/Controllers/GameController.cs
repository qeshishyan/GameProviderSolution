using CrashGameService.Service;
using CrashGameService.Service.Models;
using CrashGameService.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrashGameService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IBetService _betService;

        public GameController(IGameService gameService, IBetService betService)
        {
            _gameService = gameService;
            _betService = betService;
        }

        [HttpPost("bet")]
        public async ValueTask<IActionResult> Bet(BetRequest request)
        {
            return Ok(await _betService.Bet(request));
        }

        [HttpPost("cashOut")]
        public async ValueTask<IActionResult> CashOut(CashOutRequest request)
        {
            return Ok(await _betService.CashOut(request));
        }

        [HttpGet("start")]
        public async ValueTask<IActionResult> StartGame()
        {
            await _gameService.StartGame();
            return Ok(new { Message = "Game started" });
        }
    }
}
