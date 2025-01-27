using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TavernHelios.Auth.DTO;
using TavernHelios.Auth.Services;

namespace TavernHelios.Auth.Controllers
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
        public IActionResult Register()
        {
            var userDTO = new HeliosUserDTO
            {
                YandexFullname = $"{User.FindFirstValue(ClaimTypes.Surname)} {User.FindFirstValue(ClaimTypes.GivenName)}",
                YandexUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                YandexEmail = User.FindFirstValue(ClaimTypes.Email),
                YandexLogin = User.FindFirstValue(ClaimTypes.Name)

            };

            var registeredUserDTO = _authService.LoginAndRegister(userDTO);

            // TODO можно заменить на красивое view
            return Ok("Вы успешно авторизовались!");
        }

    }
}
