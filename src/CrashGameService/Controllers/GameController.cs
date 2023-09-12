using AutoMapper;
using CrashGameService.Entities;
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
        private readonly IMapper _mapper;
        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpPost("bet")]
        public async Task<IActionResult> Bet(BetRequest request)
        {
            var bet = _mapper.Map<Bet>(request);
            return Ok(await _gameService.Bet(bet));
        }

        [HttpPost("cashOut")]
        public async Task<IActionResult> CashOut()
        {
            return Ok();
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame()
        {
            await _gameService.StartGame();
            return Ok();
        }
    }
}
