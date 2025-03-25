namespace TavernHelios.Auth.Data.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public RoleEnum RoleId { get; set; }
        public Role Role { get; set; }
    }
}
