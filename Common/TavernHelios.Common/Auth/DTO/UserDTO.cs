using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TavernHelios.Common.Auth.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public List<UserRoleDTO> UserRoles { get; set; } = new();
        public bool IsAdmin { get; set; }
    }
}
