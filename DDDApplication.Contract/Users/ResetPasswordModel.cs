namespace DDDApplication.Contract.Users
{
    public class ResetPasswordModel
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
