using System.Text.Json;
using TavernHelios.Common.Auth.DTO;
using TavernHelios.Server.Exceptions;

namespace TavernHelios.Server.Services
{
    public interface IAuthAPIService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);
        Task RegisterAsync(RegisterDTO registerDTO);
    }

    public class AuthAPIService : IAuthAPIService
    {
        private readonly HttpClient _httpClient;

        public AuthAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDTO);
            
            if (!response.IsSuccessStatusCode) throw new CustomException("Неверный логин или пароль");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            user.IsAdmin = user.UserRoles.Any(x => x.RoleName == "Admin");
            return user;
        }

        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDTO);
            if (!response.IsSuccessStatusCode) throw new CustomException("Произошла ошибка при регистрации");
            return;
        }
    }
}
