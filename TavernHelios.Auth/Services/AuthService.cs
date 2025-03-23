using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TavernHelios.Auth.Data;
using TavernHelios.Auth.Data.Models;
using TavernHelios.Common.Auth.DTO;

namespace TavernHelios.Auth.Services
{
    public interface IAuthService
    {
        void RegisterUser(RegisterDTO registerDTO);
        UserDTO LoginUser(LoginDTO loginDTO);
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
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, registerDTO.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public UserDTO LoginUser(LoginDTO loginDTO)
        {
            var existingUser = _context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefault(x => x.Login == loginDTO.Login) 
                ?? throw new Exception("Не найден пользователь с указанным логином");

            var passwordHash = _passwordHasher.HashPassword(existingUser, loginDTO.Password);
            var verificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, loginDTO.Password);
            if (verificationResult != PasswordVerificationResult.Success) throw new Exception("Указан неверный пароль");

            return new UserDTO
            {
                Id = existingUser.Id,
                FullName = existingUser.FullName,
                Login = existingUser.Login,
                UserRoles = existingUser.UserRoles
                    .Select(x => new UserRoleDTO
                    {
                        UserId = x.UserId,
                        RoleId = (int)x.RoleId,
                        RoleName = x.Role.Name
                    })
                    .ToList()
            };
        }
    }
}
