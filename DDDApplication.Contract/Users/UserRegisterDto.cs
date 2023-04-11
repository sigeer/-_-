using DDDApplication.Contract.Auth;

namespace DDDApplication.Contract.Users
{
    public class UserRegisterDto : VerifyCodeValidModel
    {
        public string? AccountName { get; set; }
        public string? Password { get; set; }
        public int Type { get; set; }
    }
}
