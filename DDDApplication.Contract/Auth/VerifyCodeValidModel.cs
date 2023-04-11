using DDDApplication.Contract.Variables;
using DDDDomain.Shared.Users;
using Utility.Constants;
using Utility.Extensions;

namespace DDDApplication.Contract.Auth
{
    public class VerifyCodeValidModel
    {
        public int KeyType { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Key
        {
            get
            {
                return GetKey(KeyType);
            }
        }
        public string? Captcha { get; set; }

        public string ValidKeyType()
        {
            if (KeyType == NotifyMethod.Email.ToInt())
            {
                if (string.IsNullOrEmpty(Email) || !Email.IsEmail())
                {
                    return UserErrorCode.EmailError;
                }
            }
            if (KeyType == NotifyMethod.Sms.ToInt())
            {
                if (string.IsNullOrEmpty(Phone) || !Phone.IsMobilePhoneNumber())
                {
                    return UserErrorCode.PhoneNumberError;
                }
            }
            return Message.Success;
        }

        public string GenerateCode()
        {
            return GenerateCode(KeyType);
        }

        public static string GenerateCode(int keyType)
        {
            if (keyType == NotifyMethod.Sms.ToInt())
                return GenerateCode();
            if (keyType == NotifyMethod.Email.ToInt())
                return GenerateCode(12, false, true);
            throw new ArgumentException($"KeyType in (1, 2)");
        }

        private static string GenerateCode(int length = 6, bool onlyNumber = true, bool hasSymbol = false)
        {
            if (onlyNumber)
                return length.GenerateRandomCode();
            return length.GenerateRandomStr(hasSymbol);
        }

        private string GetKey(int keyType)
        {
            string? key = string.Empty;
            switch (keyType)
            {
                case (int)NotifyMethod.Email:
                    key = Email;
                    break;
                case (int)NotifyMethod.Sms:
                    key = Phone;
                    break;
                default:
                    throw new ArgumentException($"KeyType in (1, 2)");
            }
            ArgumentNullException.ThrowIfNullOrEmpty(key);
            return key;
        }
    }
}
