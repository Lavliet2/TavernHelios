using Microsoft.AspNetCore.Mvc;
using TavernHelios.Auth.Services;
using TavernHelios.Common.Auth.DTO;

namespace TavernHelios.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            _authService.RegisterUser(registerDTO);
            return Ok();
        }
    }
}
