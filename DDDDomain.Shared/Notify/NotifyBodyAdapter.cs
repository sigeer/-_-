using Utility.CaptchaValidation;
using Utility.Notify;
using Utility.Notify.Email;

namespace DDDDomain.Shared.Notify
{
    public class NotifyBodyAdapter : INotifyBodyAdapter
    {
        public NotifyBody SerilizeBody(Type typeOfNotifyBase, TemplateType model, params string[] data)
        {
            if (typeOfNotifyBase == typeof(EmailNotify))
                return new EmailMessageBody() { Title = model.Title, Content = string.Format(model.TemplateData!, data[0]) };

            throw new Exception("不支持的NotifyType");
        }
    }
}
