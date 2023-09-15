using Microsoft.AspNetCore.Mvc;

namespace GameProviderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProviderController : ControllerBase
{
    [HttpGet(Name = "games")]
    public async Task<IActionResult> GetGames()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet(Name = "launch")]
    public async Task<IActionResult> LaunchGame()
    {
        await Task.CompletedTask;
        return Ok();
    }
}