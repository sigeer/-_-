namespace DDDDomain.Shared.Users
{
    public class UserConstants
    {
        public const string NickNameRegStr = """^[^\s'"$<>]*$""";
        public const string IdNoReg = @"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)";
    }
}
