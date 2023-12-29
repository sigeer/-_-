namespace DDDDomain.Shared.Users
{
    public class AuthUserInfoModel
    {
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Type { get; set; }
    }
}
