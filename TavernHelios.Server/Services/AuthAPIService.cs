using System.Text.Json;
using TavernHelios.Common.Auth.DTO;
using TavernHelios.Server.Exceptions;

namespace TavernHelios.Server.Services
{
    public interface IAuthAPIService
    {
        Task<UserDTO> LoginAsync(LoginDTO loginDTO);
        Task RegisterAsync(RegisterDTO registerDTO);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> CreateUserAsync(UserCreateDTO userDTO);
        Task<UserDTO> UpdateUserAsync(int userId, UserUpdateDTO userDTO);
        Task DeleteUserAsync(int userId);
        Task UpdateUserRolesAsync(int userId, List<int> roleIds);
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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/user");
            if (!response.IsSuccessStatusCode) throw new CustomException("Ошибка при получении списка пользователей");
            
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<IEnumerable<UserDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var user in users)
            {
                user.IsAdmin = user.UserRoles.Any(x => x.RoleName == "Admin");
            }

            return users;
        }

        public async Task<UserDTO> CreateUserAsync(UserCreateDTO userDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", userDTO);
            if (!response.IsSuccessStatusCode) throw new CustomException("Ошибка при создании пользователя");
            
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            user.IsAdmin = user.UserRoles.Any(x => x.RoleName == "Admin");
            return user;
        }

        public async Task<UserDTO> UpdateUserAsync(int userId, UserUpdateDTO userDTO)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}", userDTO);
            if (!response.IsSuccessStatusCode) throw new CustomException("Ошибка при обновлении пользователя");
            
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDTO>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            user.IsAdmin = user.UserRoles.Any(x => x.RoleName == "Admin");
            return user;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"api/user/{userId}");
            if (!response.IsSuccessStatusCode) throw new CustomException("Ошибка при удалении пользователя");
        }

        public async Task UpdateUserRolesAsync(int userId, List<int> roleIds)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}/roles", roleIds);
            if (!response.IsSuccessStatusCode) throw new CustomException("Ошибка при обновлении ролей пользователя");
        }
    }
}
