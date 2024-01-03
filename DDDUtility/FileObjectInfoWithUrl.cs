using Utility.Files;

namespace DDDUtility
{
    public class FileObjectInfoWithUrl : FileObjectInfo
    {
        public string Url
        {
            get
            {
                return AppSettings.GetFullPath(RelativePath);
            }
        }
        public string Path
        {
            get => RelativePath;
            set => RelativePath = value;
        }
    }
}
