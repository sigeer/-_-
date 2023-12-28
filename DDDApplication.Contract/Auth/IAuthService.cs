using DDDApplication.Contract.Users;
using System.Security.Claims;

namespace DDDApplication.Contract.Auth
{
    public interface IAuthService
    {
        public (string, Claim[]?) Login(string username, string password, bool isAdmin, string ip);
        string ResetPasswordSubmit(ResetPasswordModel model, int UserId);
        #region 通过验证码重置密码
        string RecoverPasswordSubmit(ResetPasswordByVerifyCode model);
        string RecoverPasswordValid(ResetPasswordByVerifyCode model);
        string SendRecoverCode(ResetPasswordByVerifyCode model);
        #endregion

        string SendLoginCode(VerifyCodeValidModel model);
        /// <summary>
        /// 通过验证码登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Claim[]? LoginCodeSubmit(VerifyCodeValidModel model);
        string Register(UserRegisterDto post);
        bool SendLoginRegister(string phone);

        #region 二次验证
        string SendCaptcha(int userId, int keyType);
        string ValidateCaptcha(int userId, int keyType, string captcha);
        bool CheckIfUserNeedValidation(int userId);

        #endregion
    }
}
