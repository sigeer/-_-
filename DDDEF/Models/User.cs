using DDDDomain.Shared.Users;
using Utility.Extensions;

namespace DDDEF.Models
{
    public partial class User
    {
        private User() { }
        public User(string? loginId, string? password, string? phoneNumber, string? name, int userType)
        {
            SetLoginId(loginId);
            UpdatePassword(password);
            UpdatePhoneNumber(phoneNumber);
            UpdateName(name);
            Type = userType;
            Status = 1;
            RegisterTime = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; private set; } = null!;
        public string LoginId { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public string? Avatar { get; private set; }
        public string? PhoneNumber { get; private set; }
        public int Type { get; private set; }
        public int Status { get; set; }
        public DateTime RegisterTime { get; }
        public string? Email { get; private set; }

        private void SetLoginId(string? loginId)
        {
            if (string.IsNullOrWhiteSpace(loginId))
                throw new BusinessException(ErrorCode.FormInvalid);

            LoginId = loginId;
        }

        public void UpdateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name) || name!.Length > 20)
                throw new BusinessException(ErrorCode.FormInvalid);

            Name = name.Trim();
        }

        public void UpdateAvatar(string? avatar)
        {
            Avatar = avatar;
        }

        public void UpdatePassword(string? password)
        {
            if (string.IsNullOrEmpty(password))
                throw new BusinessException(ErrorCode.FormInvalid);

            Password = password;
        }

        public void UpdatePhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(PhoneNumber))
                throw new BusinessException(UserErrorCode.PhoneNumberRequired);

            if (!string.IsNullOrEmpty(phoneNumber) && !phoneNumber.IsMobilePhoneNumber())
                throw new BusinessException(UserErrorCode.PhoneNumberError);

            PhoneNumber = phoneNumber;
        }

        public void UpdateEmail(string? email)
        {
            if (string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(Email))
                throw new BusinessException(UserErrorCode.EmailRequired);

            if (!string.IsNullOrEmpty(email) && !email.IsEmail())
                throw new BusinessException(UserErrorCode.EmailError);

            Email = email;
        }
    }
}
