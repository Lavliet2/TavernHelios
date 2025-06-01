using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TavernHelios.Common.Auth.DTO;
using TavernHelios.Server.Services;

namespace TavernHelios.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthAPIService _authService;

        public UsersController(IAuthAPIService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userDTO)
        {
            var user = await _authService.CreateUserAsync(userDTO);
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDTO userDTO)
        {
            var user = await _authService.UpdateUserAsync(userId, userDTO);
            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _authService.DeleteUserAsync(userId);
            return Ok();
        }

        [HttpPut("{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles(int userId, [FromBody] List<int> roleIds)
        {
            await _authService.UpdateUserRolesAsync(userId, roleIds);
            return Ok();
        }
    }
} 