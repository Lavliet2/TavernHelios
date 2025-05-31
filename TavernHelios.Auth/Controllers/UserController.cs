using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TavernHelios.Auth.Services;
using TavernHelios.Common.Auth.DTO;

namespace TavernHelios.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _authService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateDTO userDTO)
        {
            var user = _authService.CreateUser(userDTO);
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserUpdateDTO userDTO)
        {
            var user = _authService.UpdateUser(userId, userDTO);
            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            _authService.DeleteUser(userId);
            return Ok();
        }

        [HttpPut("{userId}/roles")]
        public IActionResult UpdateUserRoles(int userId, [FromBody] List<int> roleIds)
        {
            _authService.UpdateUserRoles(userId, roleIds);
            return Ok();
        }
    }
} 