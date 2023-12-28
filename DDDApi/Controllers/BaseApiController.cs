using DDDApplication.Contract.Documents;
using DDDUtility;
using Microsoft.AspNetCore.Mvc;
using Utility.Constants;
using Utility.Extensions;
using Utility.Files;

namespace DDDApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int UserId => User.Identity.GetUserId();


        protected async Task<(string, FileObjectInfoWithUrl?)> UploadFile(FileManager fileManager, IFormFile file, Func<string, long, string> fileValidFunc, DocumentType docType)
        {
            if (file == null)
            {
                return (Message.Error, null);
            }
            var validResult = fileValidFunc(file.FileName, file.Length);
            if (validResult != Message.Success)
            {
                return (validResult, null);
            }
            using (var s = file.OpenReadStream())
            {
                var savedInfo = await fileManager.UploadAsync(docType.ToString(), file.FileName, s);
                if (savedInfo == null)
                {
                    return (Message.Error, null);
                }
                else
                {
                    return (Message.Success, new FileObjectInfoWithUrl
                    {
                        DisplayName = savedInfo.DisplayName,
                        FileName = savedInfo.FileName,
                        RelativePath = savedInfo.RelativePath,
                    });
                }
            }
        }
    }
}
