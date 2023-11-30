namespace DDDDomain.Shared.BusinessErrorCode
{
    public static class ErrorCode
    {
        /// <summary>
        /// 因代码或手动处理导致了数据出现不在预期范围内的值
        /// </summary>
        public const string DataFault = "DataFault";

        public const string StatusLocked = "StatusLocked";

        public const string LoginInvalid = "LoginInvalid";
        public const string FormInvalid = "FormInvalid";
        public const string DataNotExist = "DataNotExist";

        public const string DataExisted = "DataExisted";
        public const string DataInUsing = "DataInUsing";

        public const string FileSizeLimited1MB = "FileSizeLimited1MB";
        public const string FileSizeLimited3MB = "FileSizeLimited3MB";
        public const string ErrorFileNameLength = "ErrorFileNameLength";
        public const string ErrorFileTypeExcel = "ErrorFileTypeExcel";
        public const string ErrorFileTypeMedia = "ErrorFileTypeMedia";

        public const string UserNotInUsing = "UserNotInUsing";
        public const string CaptchaError = "CaptchaError";
        public const string UserNotExist = "UserNotExist";

        public const string OldPasswordError = "OldPasswordError";
        public const string NewPasswordError = "NewPasswordError";

        public const string DataInvalid = "NoValidData";

        public const string OnlyOwnerCanEdit = "OnlyOwnerCanEdit";

        public static string ReturnMergeData(this string errorCode, params string[] mergedValues)
        {
            return errorCode + "|" + string.Join('|', mergedValues);
        }
    }
}
