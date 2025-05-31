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
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO CreateUser(UserCreateDTO userDTO);
        UserDTO UpdateUser(int userId, UserUpdateDTO userDTO);
        void DeleteUser(int userId);
        void UpdateUserRoles(int userId, List<int> roleIds);
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
            if (registerDTO.Password != registerDTO.ConfirmPassword) throw new Exception("Пароли не совпадают");
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
            var roles = existingUser.UserRoles.Select(r => (int)r.RoleId).ToList();

            var userRoles = existingUser.UserRoles
                    .Select(x => new UserRoleDTO
                    {
                        UserId = x.UserId,
                        RoleId = (int)x.RoleId,
                        RoleName = x.Role.Name
                    }).ToList();

            return new UserDTO
            {
                Id = existingUser.Id,
                FullName = existingUser.FullName,
                Login = existingUser.Login,
                UserRoles = userRoles,
                Roles = roles
            };
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Select(x => new UserDTO
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Login = x.Login,
                    UserRoles = x.UserRoles.Select(r => new UserRoleDTO
                    {
                        UserId = r.UserId,
                        RoleId = (int)r.RoleId,
                        RoleName = r.Role.Name
                    }).ToList()
                })
                .ToList();
        }

        public UserDTO CreateUser(UserCreateDTO userDTO)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Login == userDTO.Login);
            if (existingUser != null) throw new Exception("Пользователь с таким логином уже существует");

            var newUser = new User
            {
                FullName = userDTO.FullName,
                Login = userDTO.Login
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userDTO.Password);

            _context.Users.Add(newUser);
            _context.SaveChanges();

            if (userDTO.RoleIds != null && userDTO.RoleIds.Any())
            {
                var userRoles = userDTO.RoleIds.Select(roleId => new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = (RoleEnum)roleId
                });
                _context.UsersRoles.AddRange(userRoles);
                _context.SaveChanges();
            }

            return new UserDTO
            {
                Id = newUser.Id,
                FullName = newUser.FullName,
                Login = newUser.Login,
                UserRoles = _context.UsersRoles
                    .Include(x => x.Role)
                    .Where(x => x.UserId == newUser.Id)
                    .Select(x => new UserRoleDTO
                    {
                        UserId = x.UserId,
                        RoleId = (int)x.RoleId,
                        RoleName = x.Role.Name
                    })
                    .ToList()
            };
        }

        public UserDTO UpdateUser(int userId, UserUpdateDTO userDTO)
        {
            var user = _context.Users
                .Include(x => x.UserRoles)
                .FirstOrDefault(x => x.Id == userId)
                ?? throw new Exception("Пользователь не найден");

            if (!string.IsNullOrEmpty(userDTO.FullName))
                user.FullName = userDTO.FullName;

            if (userDTO.RoleIds != null)
            {
                var existingRoles = user.UserRoles.ToList();
                _context.UsersRoles.RemoveRange(existingRoles);

                var newRoles = userDTO.RoleIds.Select(roleId => new UserRole
                {
                    UserId = userId,
                    RoleId = (RoleEnum)roleId
                });
                _context.UsersRoles.AddRange(newRoles);
            }

            _context.SaveChanges();

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Login = user.Login,
                UserRoles = _context.UsersRoles
                    .Include(x => x.Role)
                    .Where(x => x.UserId == user.Id)
                    .Select(x => new UserRoleDTO
                    {
                        UserId = x.UserId,
                        RoleId = (int)x.RoleId,
                        RoleName = x.Role.Name
                    })
                    .ToList()
            };
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users
                .Include(x => x.UserRoles)
                .FirstOrDefault(x => x.Id == userId)
                ?? throw new Exception("Пользователь не найден");

            _context.UsersRoles.RemoveRange(user.UserRoles);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void UpdateUserRoles(int userId, List<int> roleIds)
        {
            var user = _context.Users
                .Include(x => x.UserRoles)
                .FirstOrDefault(x => x.Id == userId)
                ?? throw new Exception("Пользователь не найден");

            var existingRoles = user.UserRoles.ToList();
            _context.UsersRoles.RemoveRange(existingRoles);

            if (roleIds != null && roleIds.Any())
            {
                var newRoles = roleIds.Select(roleId => new UserRole
                {
                    UserId = userId,
                    RoleId = (RoleEnum)roleId
                });
                _context.UsersRoles.AddRange(newRoles);
            }

            _context.SaveChanges();
        }
    }
}
