using Utility.Extensions;

namespace DDDUtility
{
    public class AppSettingsItem
    {
        public const string MySqlConnectStr = "MySql";
        public const string PostgreSql = "PostgreSql";
        public const string FileUrl = "FileUrl";
        public const string RedisHost = "RedisHost";
    }
    public class AppSettings
    {
        public static string FileUrl { get; set; } = null!;
        public static string APIUrl { get; set; } = null!;
        public static string LayoutCDNUrl { get; set; } = null!;

        public static List<string> RichTextEditorReplaceFields { get; set; } = new List<string>();
        public static string RichTextEditorReplaceValue { get; set; } = null!;


        public static string GetFullPath(string relatePath)
        {

            if (string.IsNullOrEmpty(relatePath))
            {
                return string.Empty;
            }
            if (relatePath.StartsWith("http"))
            {
                return relatePath;
            }
            return FileUrl.TrimEnd('/') + '/' + relatePath.ToRelativeUrlString();
        }

        public static string UploadRootDir = "Upload";
        public static List<string> Receiver = new List<string>();

        /// <summary>
        /// workdir/Upload/
        /// </summary>
        /// <returns></returns>
        public static string GetUploadRootDir()
        {
            return Path.Combine(Environment.CurrentDirectory, UploadRootDir);
        }

        /// <summary>
        /// UploadRootDir + relativePath
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetUploadPhysicalPath(string relativePath)
        {
            return Path.Combine(GetUploadRootDir(), relativePath.TrimStart('/'));
        }

        public static string ProcessRichTextContent(string str)
        {
            RichTextEditorReplaceFields.ForEach(x =>
            {
                str = str.Replace(x, RichTextEditorReplaceValue);
            });
            return str;
        }

        public static string FormatLayoutCDN(string relativeUrl)
        {
            return LayoutCDNUrl + relativeUrl.TrimStart('~');
        }
    }
}
