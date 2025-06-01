namespace TavernHelios.Auth.Data.Models
{
    public class Role
    {
        public RoleEnum Id { get; set; }
        public string Name { get; set; }
    }

    public enum RoleEnum
    {
        Admin = 1,
        Manager = 2
    }
}
