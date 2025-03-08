using Microsoft.AspNetCore.Identity;
using TavernHelios.Auth.Data;
using TavernHelios.Auth.Models;
using TavernHelios.Common.Auth.DTO;

namespace TavernHelios.Auth.Services
{
    public interface IAuthService
    {
        void RegisterUser(RegisterDTO registerDTO);
    }

    public class AuthService : IAuthService
    {
        private readonly EfDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AuthService(EfDbContext context)
        {
            _context = context;
        }
        public void RegisterUser(RegisterDTO registerDTO)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Login == registerDTO.Login);
            if (existingUser != null) throw new Exception("Пользователь с таким логином уже зарегистрирован");

            var newUser = new User
            {
                FullName = registerDTO.FullName,
                Login = registerDTO.Login
            };
            newUser.PasswordHash = HashPassword(newUser, registerDTO.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }


        private string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }
    }
}
