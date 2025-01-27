using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using TavernHelios.Server.DTO;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        [HttpGet("info")]
        public IActionResult GetUserInfo()
        {
            return Redirect("https://localhost:7113/users/info");
        }
    }
}
