using System.Text.Json;
using TavernHelios.Common.Auth.DTO;

namespace TavernHelios.Server.Services
{
    public interface IAuthAPIService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);
        Task<bool> RegisterAsync(RegisterDTO registerDTO);
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
            
            if (!response.IsSuccessStatusCode) throw new Exception("Ошибка аутентификации");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return user;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDTO);

            return response.IsSuccessStatusCode;
        }
    }
}
