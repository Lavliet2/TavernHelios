using Microsoft.AspNetCore.Mvc;
using TavernHelios.Common.Auth.DTO;
using TavernHelios.Server.Services;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAPIService _authService;
        public AuthController(IAuthAPIService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            await _authService.RegisterAsync(registerDTO);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            // TODO
            return Ok();
        }
    }
}
