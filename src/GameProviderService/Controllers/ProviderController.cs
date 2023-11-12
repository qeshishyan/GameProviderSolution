using GameProviderService.Service;
using GameProviderService.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameProviderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProviderController : ControllerBase
{
    private readonly IProviderService _providerService;
    public ProviderController(IProviderService providerService)
    {
        _providerService = providerService;
    }
    [HttpGet("games")]
    public async Task<IActionResult> GetGames()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("launch")]
    public async Task<IActionResult> LaunchGame(LaunchRequest request)
    {
        LaunchResponse result = await _providerService.LaunchGame(request);
        return Ok(result);
    }
}