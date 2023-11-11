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
            //Get user token from Header token 
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
            var respone = await _gameService.StartGame();
            return Ok(respone);
        }

        [HttpPost("getLastMultipliers")]
        public async ValueTask<IActionResult> GetLastMultipliers(GetLastMultiplierRequest request)
        {
            var response = await _gameService.GetLastMultipliers(request.SessionId, request.SessionId);
            return Ok(response);
        }

        [HttpPost("getLastBets")]
        public async ValueTask<IActionResult> GetLastBets(GetLastBetsRequest request)
        {
            var response = await _gameService.GetLastBets(request.SessionId, request.SessionId);
            return Ok(response);
        }
    }
}
