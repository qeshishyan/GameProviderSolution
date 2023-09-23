using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("getUser")]
        public async Task<IActionResult> GetUser(GetUserRequest request)
        {
            return Ok(await _userService.GetUser(request));
        }

        [HttpPost("getUserBalance")]
        public async Task<IActionResult> GetUserBalance(GetUserRequest request)
        {
            return Ok(await _userService.GetUserBalance(request));
        }
    }
}
