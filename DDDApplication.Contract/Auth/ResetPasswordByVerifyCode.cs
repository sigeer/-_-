namespace DDDApplication.Contract.Auth
{
    public class ResetPasswordByVerifyCode : VerifyCodeValidModel
    {
        public string? AccountName { get; set; }
        public string? Password { get; set; }
    }
}
