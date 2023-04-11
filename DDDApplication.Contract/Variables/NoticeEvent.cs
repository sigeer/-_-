namespace DDDApplication.Contract.Variables
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NoticeEventAttribute : Attribute
    {
        public NoticeEventAttribute(int eventLevel, string content)
        {
            EventLevel = eventLevel;
            Content = content;
        }

        public int EventLevel { get; set; }
        public string Content { get; set; } = null!;
    }
}
