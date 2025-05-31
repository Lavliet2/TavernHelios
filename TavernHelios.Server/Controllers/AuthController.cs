using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TavernHelios.Common.Auth.DTO;
using TavernHelios.Server.Services;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _authService.LoginAsync(loginDTO);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Name, user.Login),
            };

            var roleClaims =
                user.UserRoles.Select(c => new Claim(ClaimTypes.Role, c.RoleName)).ToList();

            claims.AddRange(roleClaims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Ok(user);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [Authorize]
        [HttpGet("userInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var fullName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var login = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var roleClaims = User.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
            
            var roles = roleClaims.Select(claim => claim.Value switch
            {
                "Admin" => 1,
                "Manager" => 2,
                "User" => 3,
                _ => 0
            }).ToList();

            return Ok(new UserDTO
            {
                FullName = fullName,
                Login = login,
                Roles = roles,
                IsAdmin = roleClaims.Any(x => x.Value == "Admin")
            });
        }
    }
}
