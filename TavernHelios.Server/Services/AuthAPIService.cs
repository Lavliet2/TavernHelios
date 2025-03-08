using TavernHelios.Common.Auth.DTO;

namespace TavernHelios.Server.Services
{
    public interface IAuthAPIService
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> RegisterAsync(RegisterDTO registerDTO);
    }

    public class AuthAPIService : IAuthAPIService
    {
        private readonly HttpClient _httpClient;

        public AuthAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var loginModel = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDTO);

            return response.IsSuccessStatusCode;
        }
    }
}
