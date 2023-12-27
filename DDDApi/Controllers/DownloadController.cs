using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Extensions;
using Utility.Files;

namespace DDDApi.Controllers
{
    [Authorize]
    [Route("Files")]
    public class DownloadController : Controller
    {
        readonly IFileManager _fileManager;
        public DownloadController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpGet("{*relativePath}")]
        public async Task<IActionResult> Index1(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return NotFound();
            var downloadObj = await _fileManager.DownloadAsync(relativePath.Split('?')[0]);
            if (downloadObj == null)
                return NotFound();

            return File(downloadObj.Stream, downloadObj.Extension.GetMimeType());
        }
    }
}
