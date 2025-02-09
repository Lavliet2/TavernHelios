using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TavernHelios.Server.DTO;
using TavernHelios.Server.Services.Auth;

namespace TavernHelios.Server.Controllers
{
    [Route("yandexAuth")]
    [ApiController]
    public class YandexAuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public YandexAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/yandexAuth/register")
        {
            // задает AuthProperties (в самый первый раз - null)

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "Yandex");
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            var userDTO = new HeliosUserDTO
            {
                YandexFullname = $"{User.FindFirstValue(ClaimTypes.Surname)} {User.FindFirstValue(ClaimTypes.GivenName)}",
                YandexUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                YandexEmail = User.FindFirstValue(ClaimTypes.Email),
                YandexLogin = User.FindFirstValue(ClaimTypes.Name)

            };

            var zxc = User.Claims.ToList();
            var identity = new ClaimsIdentity(zxc, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);


            var registeredUserDTO = _authService.LoginAndRegister(userDTO);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            // TODO можно заменить на красивое view
            return Ok("Вы успешно авторизовались!");
        }

    }
}
