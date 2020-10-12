using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace VideoLibrary.API.Controllers
{
    [Route("api/videoFiles")]

    public class VideoFilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public VideoFilesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{videoFile}")]
        public IActionResult GetVideo(string videoFile)
        {
            var mime = videoFile.Split('.').Last();
            var savePath = Path.Combine(_env.WebRootPath, videoFile);

            return  PhysicalFile(savePath, "videoFile/*", enableRangeProcessing: true);
            //return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read), 
            //    "videoFile/*");
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> UploadVideoFile(IFormFile video)
        {
            var mime = video.FileName.Split('.').Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await video.CopyToAsync(fileStream);
            }
                return Ok(fileName);
        }
    }
}