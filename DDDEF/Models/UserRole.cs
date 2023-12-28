namespace DDDEF.Models
{
    public class UserRole
    {
        private UserRole()
        {
        }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
