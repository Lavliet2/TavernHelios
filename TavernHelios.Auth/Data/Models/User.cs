namespace TavernHelios.Auth.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public List<UserRole> UserRoles { get; set; } = new();
    }
}
