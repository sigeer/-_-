namespace DDDEF.Models
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string PermissionKey { get; set; } = null!;
    }
}
