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
        [HttpGet("preview/{previewImage}")]
        public IActionResult GetPreviewImage(string previewImage)
        {
            var savePath = Path.Combine(_env.WebRootPath, previewImage);

            //return PhysicalFile(savePath, "preview/*");
            return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read),
                "preview/*");
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> UploadVideoFile(IFormFile video)
        {
            var mime = video.FileName.Split('.').Last();
            var fileName = string.Concat($"temp_{DateTime.Now.Ticks}", ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await video.CopyToAsync(fileStream);
            }
       

            return Ok(fileName);
            //return Ok(new ResponseMessage {FileNameConvert = fileName, DurationConvert = conversionResult.Duration.ToString() });
        }   

        [HttpPost("research")]
        public async Task<IActionResult> ConvertResearch()
        {
            var originalFile = "VID20200821180752.mp4";
            var fileName = Path.GetRandomFileName();
            var targetResearchPath = Path.Combine(_env.WebRootPath, originalFile);

            string executablesPath = Path.Combine(_env.ContentRootPath, "FFmpeg");
            FFmpeg.SetExecutablesPath(executablesPath);

            string convertFileName = fileName + "FixedBitrate_VP9+Opus_1080_FFmpeg" + ".webm";
          
            string outputPath = Path.Combine(_env.WebRootPath, convertFileName);
            
            IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(targetResearchPath);
            IConversion conversion = FFmpeg.Conversions.New();
            conversion.OnProgress += (sender, args) =>
            {
                var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                Console.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
            };
            //IStream videoStream = mediaInfo.VideoStreams.FirstOrDefault()
            //        ?.SetCodec(VideoCodec.vp9)
            //        ?.SetSize(VideoSize.Hd1080)
            //        ?.SetFramerate(30);

            //IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()
            //        ?.SetCodec(AudioCodec.opus)
            //        ;
            //IConversionResult conversionResult = await conversion
            //    .AddStream(videoStream)
                
            //    .AddParameter("-strict -2", ParameterPosition.PostInput)
            //    .SetOutput(outputPath)
            //    .Start();
            //--------------------------------------------------------------

            convertFileName = fileName + "FixedBitrate_h264_1080_FFmpeg" + ".mp4";
            outputPath = Path.Combine(_env.WebRootPath, convertFileName);

            IStream videoStreamAV = mediaInfo.VideoStreams.FirstOrDefault()
                    ?.SetCodec(VideoCodec.h264)
                    ?.SetSize(VideoSize.Hd1080)
                    ?.SetFramerate(30);

            IStream audioStreamAV = mediaInfo.AudioStreams.FirstOrDefault()
                    ?.SetCodec(AudioCodec.opus);

            IConversionResult conversionResultAV = await conversion
                .AddStream(videoStreamAV)
                .AddParameter("-strict -2", ParameterPosition.PostInput)
                .SetOutput(outputPath)
                .Start();
            ////------------------------------------------------------------
            //convertFileName = fileName + "_VP9+opus_480_FFmpeg" + ".webm";
            //outputPath = Path.Combine(_env.WebRootPath, convertFileName);

            //IStream videoStreamVP = mediaInfo.VideoStreams.FirstOrDefault()
            //        ?.SetCodec(VideoCodec.vp9)
            //        ?.SetSize(VideoSize.Hd480)
            //        ?.SetFramerate(30);
            //IStream audioStreamVP = mediaInfo.AudioStreams.FirstOrDefault()
            //        ?.SetCodec(AudioCodec.opus);

            //IConversionResult conversionResultVP = await FFmpeg.Conversions.New()
            //    .AddStream(audioStreamVP, videoStreamVP)
            //    .AddParameter("-strict -2", ParameterPosition.PostInput)
            //    .SetOutput(outputPath)
            //    .Start();

            //convertFileName = fileName + "_VP9+opus_uhd2160_FFmpeg" + ".webm";
            //outputPath = Path.Combine(_env.WebRootPath, convertFileName);

            //IStream videoStreamVPP = mediaInfo.VideoStreams.FirstOrDefault()
            //        ?.SetCodec(VideoCodec.vp9)
            //        ?.SetSize(VideoSize.Uhd2160)
            //        ?.SetFramerate(30);
            //IStream audioStreamVPP = mediaInfo.AudioStreams.FirstOrDefault()
            //        ?.SetCodec(AudioCodec.opus);

            //IConversionResult conversionResultVPP = await FFmpeg.Conversions.New()
            //    .AddStream(audioStreamVPP, videoStreamVPP)
            //    .AddParameter("-strict -2", ParameterPosition.PostInput)
            //    .SetOutput(outputPath)
            //    .Start();

            //convertFileName = fileName + "_VP9+opus_2K_FFmpeg" + ".webm";
            //outputPath = Path.Combine(_env.WebRootPath, convertFileName);

            //videoStreamVPP = mediaInfo.VideoStreams.FirstOrDefault()
            //       ?.SetCodec(VideoCodec.vp9)
            //       ?.SetSize(2560, 1440)
            //       ?.SetFramerate(30);
            //audioStreamVPP = mediaInfo.AudioStreams.FirstOrDefault()
            //       ?.SetCodec(AudioCodec.opus);

            //conversionResultVPP = await FFmpeg.Conversions.New()
            //   .AddStream(audioStreamVPP, videoStreamVPP)
            //   .AddParameter("-strict -2", ParameterPosition.PostInput)
            //   .SetOutput(outputPath)
            //   .Start();

            return Ok();
        }
        public class ResponseMessage
        {
            public string FileNameConvert { get; set; }
            public string DurationConvert { get; set; }
        }
    }


}