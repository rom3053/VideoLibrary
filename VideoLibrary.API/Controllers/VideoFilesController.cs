using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Xabe.FFmpeg;

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

           string outputPath = Path.Combine(_env.WebRootPath, fileName + "FFmpeg" + ".mp4");
           string executablesPath = Path.Combine(_env.ContentRootPath,"FFmpeg");
           FFmpeg.SetExecutablesPath(executablesPath);
           //IConversion result = await FFmpeg.Conversions.FromSnippet.ToWebM(savePath, outputPath);
           //IConversionResult conversionResult = await result.Start();

            IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(savePath);
            IStream videoStream = mediaInfo.VideoStreams.FirstOrDefault()
                ?.SetCodec(VideoCodec.h264)
                ?.SetSize(VideoSize.Hd480)
                ?.SetFramerate(29.97);
            IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()
                ?.SetCodec(AudioCodec.aac);
            IConversionResult conversionResult = await FFmpeg.Conversions.New()
                .AddStream(audioStream, videoStream)
                .SetOutput(outputPath)
                .Start();

            return Ok(fileName);
        }
    }
}