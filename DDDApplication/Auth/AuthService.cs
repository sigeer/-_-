using DDDApplication.Contract.Auth;
using DDDApplication.Contract.Users;
using DDDApplication.Contract.Variables;
using DDDDomain.Shared.Common;
using DDDDomain.Shared.Notify;
using DDDDomain.Shared.Users;
using DDDDomain.Users;
using DDDEF;
using DDDEF.Models;
using System.Security.Claims;
using Utility.CaptchaValidation;
using Utility.Constants;
using Utility.Extensions;


namespace DDDApplication.Auth
{
    public class AuthService : IAuthService
    {
        readonly StorageDbContext _dbContext;
        readonly ICaptchaProcessor _notifyService;
        readonly UserManager _userManager;

        public AuthService(StorageDbContext dbContext, ICaptchaProcessor notifyService, UserManager userManager)
        {
            _dbContext = dbContext;
            _notifyService = notifyService;
            _userManager = userManager;
        }

        public AuthUserInfoDto? GetUserInfo(int userid)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userid);
            if (user != null)
            {
                var groupdata = _dbContext.UserRoles.Where(u => u.UserId == userid).ToList();
                var model = new AuthUserInfoDto()
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    Name = user.Name,
                    Email = user.Email,
                    Type = user.Type,
                    Group = groupdata.Select(x => new UserGroupDto()
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        GroupId = x.RoleId
                    }).ToList()

                };
                return model;
            }
            return null;
        }
        public (string, Claim[]?) Login(string username, string password, bool isAdmin, string ip)
        {
            if (!isAdmin)
                return ClientLogin(username, password, ip);
            return ClientLogin(username, password, ip);
        }

        private (string, Claim[]?) ClientLogin(string username, string password, string ip)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.LoginId == username);

            if (user != null)
            {
                if (user.Password?.ToUpper() == password.ToUpper())
                {
                    if (user.Status == 1)
                    {
                        return (Message.Success, GetClaims(user.Id, user.Type));
                    }
                    else
                        return (LoginCode.AccountForbidden, null);
                }

            }
            return (LoginCode.LoginFormInvalid, null);
        }

        private static Claim[] GetClaims(int userId, int userType)
        {
            return
                new Claim[]{
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(ClaimTypes.Role, userType.ToString())
                };
        }

        public string ResetPasswordSubmit(ResetPasswordModel model, int UserId)
        {
            var obj = _dbContext.Users.Find(UserId);
            if (obj == null)
                return ErrorCode.UserNotExist;
            if (obj.Password.Equals(model.OldPassword, StringComparison.OrdinalIgnoreCase))
            {
                obj.UpdatePassword(model.NewPassword.ToUpper());
                return _dbContext.SaveChanges() > 0 ? Message.Success : Message.Error;
            }
            return ErrorCode.OldPasswordError;
        }

        public string RecoverPasswordSubmit(ResetPasswordByVerifyCode model)
        {
            var validResult = model.ValidKeyType();
            if (validResult != Message.Success)
                return validResult;

            User? user = null;
            if (model.KeyType == NotifyMethod.Email.ToInt())
                user = _dbContext.Users.FirstOrDefault(x => x.Email == model.Email);
            if (model.KeyType == NotifyMethod.Sms.ToInt())
                user = _dbContext.Users.FirstOrDefault(x => x.PhoneNumber == model.Phone);
            if (user == null)
                return ErrorCode.UserNotExist;

            if (_notifyService.IfNeedValidation(model.Key, VerifyCodePurpose.Recover))
                return VerifyCodeErrorCode.SessionTimeOut;

            user.UpdatePassword(model.Password);
            _dbContext.SaveChanges();
            _notifyService.RemoveSession(model.Key, VerifyCodePurpose.Recover);
            return Message.Success;
        }

        public string RecoverPasswordValid(ResetPasswordByVerifyCode model)
        {
            var validResult = model.ValidKeyType();
            if (validResult != Message.Success)
                return validResult;
            return _notifyService.Validate(model.Key, model.Captcha, VerifyCodePurpose.Recover) ? Message.Success : ErrorCode.CaptchaError;
        }

        public string SendRecoverCode(ResetPasswordByVerifyCode model)
        {
            var validResult = model.ValidKeyType();
            if (validResult != Message.Success)
                return validResult;

            if (model.KeyType == NotifyMethod.Email.ToInt() && !_dbContext.Users.Any(x => x.Email == model.Email && x.LoginId == model.AccountName))
                return Message.Error;
            if (model.KeyType == NotifyMethod.Sms.ToInt() && !_dbContext.Users.Any(x => x.PhoneNumber == model.Phone && x.LoginId == model.AccountName))
                return Message.Error;

            return _notifyService.SendCaptcha(model.Key, model.GenerateCode(), VerifyCodePurpose.Recover) ? Message.Success : Message.Error;
        }

        public string SendLoginCode(VerifyCodeValidModel model)
        {
            var validResult = model.ValidKeyType();
            if (validResult != Message.Success)
                return validResult;

            if (model.KeyType == NotifyMethod.Email.ToInt() && !_dbContext.Users.Any(x => x.Email == model.Email))
                return Message.Error;
            if (model.KeyType == NotifyMethod.Sms.ToInt() && !_dbContext.Users.Any(x => x.PhoneNumber == model.Phone))
                return Message.Error;

            return _notifyService.SendCaptcha(model.Key, model.GenerateCode(), VerifyCodePurpose.Login) ? Message.Success : Message.Error;
        }

        public Claim[]? LoginCodeSubmit(VerifyCodeValidModel model)
        {
            var validResult = model.ValidKeyType();
            if (validResult != Message.Success)
                return null;

            if (!_notifyService.IfNeedValidation(model.Key, VerifyCodePurpose.Recover) || _notifyService.Validate(model.Key, model.Captcha, VerifyCodePurpose.Login))
            {
                User? user = null;
                if (model.KeyType == NotifyMethod.Email.ToInt())
                    user = _dbContext.Users.FirstOrDefault(x => x.Email == model.Email);
                if (model.KeyType == NotifyMethod.Sms.ToInt())
                    user = _dbContext.Users.FirstOrDefault(x => x.PhoneNumber == model.Phone);
                if (user == null)
                    return null;

                _notifyService.RemoveSession(model.Key, VerifyCodePurpose.Login);
                return GetClaims(user.Id, user.Type);
            }
            return null;
        }

        public string Register(UserRegisterDto post)
        {
#if !DEBUG
            if (!_notifyService.NeedValidation(post.Key, post.KeyType, VerifyCodePurpose.Register) || _notifyService.Validate(post.Key, post.Captcha, post.KeyType, VerifyCodePurpose.Register))
            {
                _dbContext.Users.Add(_userManager.RegisterUser(post.AccountName, post.Password, post.Phone, post.Type));
                _dbContext.SaveChanges();

                _notifyService.RemoveSession(post.Key, VerifyCodePurpose.Register);
                return Message.Success;
            }
            return VerifyCodeErrorCode.CodeError;
#else
            _dbContext.Users.Add(_userManager.RegisterUser(post.AccountName, post.Password, post.Phone, post.Type));
            _dbContext.SaveChanges();
            return Message.Success;
#endif
        }
        public bool SendLoginRegister(string phone)
        {
#if !DEBUG
            return _notifyService.SendCaptcha(phone, 6.GenerateRandomCode(), VerifyCodePurpose.Register);
#else
            return true;
#endif
        }

        public string SendCaptcha(int userId, int keyType)
        {
            var userModel = _dbContext.Users.Find(userId);
            if (userModel == null)
                return UserErrorCode.DataNotExsited;

            string? key = string.Empty;
            if (keyType == NotifyMethod.Email.ToInt())
                key = userModel.Email;
            else if (keyType == NotifyMethod.Sms.ToInt())
                key = userModel.PhoneNumber;

            if (string.IsNullOrEmpty(key))
                return ErrorCode.FormInvalid;

            return _notifyService.SendCaptcha(key, VerifyCodeValidModel.GenerateCode(keyType), VerifyCodePurpose.DoubleCheck) ? Message.Success : Message.Error;
        }

        public string ValidateCaptcha(int userId, int keyType, string captcha)
        {
            var userModel = _dbContext.Users.Find(userId);
            if (userModel == null)
                return UserErrorCode.DataNotExsited;

            string? key = string.Empty;
            if (keyType == NotifyMethod.Email.ToInt())
                key = userModel.Email;
            else if (keyType == NotifyMethod.Sms.ToInt())
                key = userModel.PhoneNumber;

            if (string.IsNullOrEmpty(key))
                return ErrorCode.FormInvalid;

            return _notifyService.Validate(key, captcha, VerifyCodePurpose.DoubleCheck) ? Message.Success : VerifyCodeErrorCode.CodeError;
        }

        public bool CheckIfUserNeedValidation(int userId)
        {
            var userModel = _dbContext.Users.Find(userId);
            if (userModel == null)
                return false;

            var checkModel = new VerifyCodeValidModel()
            {
                Phone = userModel.PhoneNumber,
                KeyType = NotifyMethod.Sms.ToInt(),
            };

            var checkResult = _notifyService.IfNeedValidation(checkModel.Key, VerifyCodePurpose.DoubleCheck);
            if (!checkResult)
                return false;

            if (!string.IsNullOrEmpty(userModel.Email))
            {
                checkModel.Email = userModel.Email;
                checkModel.KeyType = NotifyMethod.Email.ToInt();
            }
            return _notifyService.IfNeedValidation(checkModel.Key, VerifyCodePurpose.DoubleCheck);
        }
    }
}
