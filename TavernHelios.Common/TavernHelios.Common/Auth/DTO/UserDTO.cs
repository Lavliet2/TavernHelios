using System.Collections.Generic;

namespace TavernHelios.Common.Auth.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public List<UserRoleDTO> UserRoles { get; set; }
        public List<int> Roles { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class UserRoleDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UserCreateDTO
    {
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<int> RoleIds { get; set; }
    }

    public class UserUpdateDTO
    {
        public string FullName { get; set; }
        public List<int> RoleIds { get; set; }
    }
}