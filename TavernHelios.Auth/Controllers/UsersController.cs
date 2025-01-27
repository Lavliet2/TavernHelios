using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TavernHelios.Auth.DTO;

namespace TavernHelios.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        [HttpGet("info")]
        public IActionResult GetUserInfo()
        {
            var claims = User.Claims.ToList();
            var identity = User.Identities.FirstOrDefault();
            if (identity == null || !identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userDTO = new HeliosUserDTO
            {
                YandexFullname = $"{User.FindFirstValue(ClaimTypes.Surname)} {User.FindFirstValue(ClaimTypes.GivenName)}",
                YandexUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                YandexEmail = User.FindFirstValue(ClaimTypes.Email),
                YandexLogin = User.FindFirstValue(ClaimTypes.Name)

            };

            return Ok(userDTO);
        }
    }
}
