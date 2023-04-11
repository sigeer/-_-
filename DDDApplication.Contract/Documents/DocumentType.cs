using System.ComponentModel;

namespace DDDApplication.Contract.Documents
{
    public enum DocumentType
    {
        [Description("UserAvatar")]
        UserAvatar = 1,
        [Description("RichTextEditor")]
        RichTextEditor = 2,
        [Description("News")]
        News = 3,
        [Description("Course")]
        Course = 4,
        [Description("Question")]
        Question = 5
    }
}
