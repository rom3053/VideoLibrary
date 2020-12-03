using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using VideoLibrary.API.Models;
using VideoLibrary.Data;
using Xabe.FFmpeg;


namespace VideoLibrary.API.BackgroundServices
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<VideoEditingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ChannelReader<EditVideoMessage> _channelReader;

        public VideoEditingBackgroundService(
            IWebHostEnvironment env,
            Channel<EditVideoMessage> channel,
            ILogger<VideoEditingBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _env = env;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _channelReader = channel.Reader;
        }
        protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReader.WaitToReadAsync(stoppingToken))
            {
                var message = await _channelReader.ReadAsync(stoppingToken);

                string executablesPath = Path.Combine(_env.ContentRootPath, "FFmpeg");
                FFmpeg.SetExecutablesPath(executablesPath);

                using var scope = _serviceProvider.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var submission = ctx.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));
                var video = ctx.Videos.FirstOrDefault(v => v.Id.Equals(submission.VideoId));

                string previewImageName = Guid.NewGuid() + ".png";
                video.PreviewImage = previewImageName;

                try
                {
                    #region ConvertRegion

                    string convertFileName = message.Output;
                    string outputPath = Path.Combine(_env.WebRootPath, convertFileName);
                    string inputPath = Path.Combine(_env.WebRootPath, message.Input);
                    string outputName = string.Empty;


                    IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(inputPath);

                    await GetSnapshot(previewImageName, inputPath);

                    ConvertOption videoSize = GetConvertOptionMaxQualityToConvert(mediaInfo);
                    switch (ConvertOption.Option240)
                    {
                        case ConvertOption.Option1080:
                            IConversionResult conversionResult_1080 = await ConversionProcessWithAlterableQuality(outputPath, mediaInfo, VideoSize.Hd1080);
                            DisplayResultToConsole(conversionResult_1080);
                            await AddQualityToDatabase(message, ctx, submission, stoppingToken, VideoSize.Hd1080);
                            if (videoSize < ConvertOption.Option1080)
                            {
                                //goto case ConvertOption.Option360;
                            }
                            DeleteSourceFile(inputPath);
                            break;
                        case ConvertOption.Option720:
                            IConversionResult conversionResult_720 = await ConversionProcessWithAlterableQuality(outputPath, mediaInfo, VideoSize.Hd720);
                            DisplayResultToConsole(conversionResult_720);
                            await AddQualityToDatabase(message, ctx, submission, stoppingToken, VideoSize.Hd720);
                            if (videoSize < ConvertOption.Option720)
                            {
                                goto case ConvertOption.Option1080;
                            }
                            DeleteSourceFile(inputPath);
                            break;
                        case ConvertOption.Option480:
                            IConversionResult conversionResult_480 = await ConversionProcessWithAlterableQuality(outputPath, mediaInfo, VideoSize.Hd480);
                            DisplayResultToConsole(conversionResult_480);
                            await AddQualityToDatabase(message, ctx, submission, stoppingToken, VideoSize.Hd480);
                            if (videoSize < ConvertOption.Option480)
                            {
                                goto case ConvertOption.Option720;
                            }
                            DeleteSourceFile(inputPath);
                            break;
                        case ConvertOption.Option360:
                            IConversionResult conversionResult_360 = await ConversionProcessWithAlterableQuality(outputPath, mediaInfo, VideoSize.Nhd);
                            DisplayResultToConsole(conversionResult_360);
                            await AddQualityToDatabase(message, ctx, submission, stoppingToken, VideoSize.Nhd);
                            if (videoSize < ConvertOption.Option360)
                            {
                                goto case ConvertOption.Option480;
                            }
                            DeleteSourceFile(inputPath);
                            break;
                        case ConvertOption.Option240:
                            IConversionResult conversionResult_240 = await ConversionProcessWithAlterableQuality(outputPath, mediaInfo, VideoSize.Fwqvga);
                            DisplayResultToConsole(conversionResult_240);
                            string videoLink = message.Output + "FFmpeg_" + VideoSize.Fwqvga + ".mp4";
                            submission.VideoFile = videoLink;
                            VideoQuality videoQuality = new VideoQuality
                            {
                                QualityName = "240",
                                Submission = submission,
                                SubmissionId = submission.Id,
                                QualityVideoLink = videoLink
                            };
                            submission.VideoQualities.Add(videoQuality);
                            await ctx.SaveChangesAsync(stoppingToken);
                            if (videoSize < ConvertOption.Option240)
                            {
                                goto case ConvertOption.Option360;
                            }
                            DeleteSourceFile(inputPath);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video Processing Failed for {0}", message.Input);
                }
                #endregion
            }
        }

        private static async Task<IConversionResult> ConversionProcessWithAlterableQuality(string outputPath, IMediaInfo mediaInfo, VideoSize videoSize)
        {
            IStream videoStream = mediaInfo.VideoStreams.FirstOrDefault()
                                    ?.SetCodec(VideoCodec.h264_nvenc)
                                    ?.SetSize(videoSize)
                                    ?.SetFramerate(24);

            IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()
                ?.SetCodec(AudioCodec.aac);



            IConversion conversion = FFmpeg.Conversions.New();
            conversion.OnProgress += (sender, args) =>
            {
                var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                Console.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
            };

            outputPath += "FFmpeg_" + videoSize + ".mp4";
            IConversionResult conversionResult = await conversion
                .AddParameter("-hwaccel_device 0 -hwaccel cuda", ParameterPosition.PreInput)
                .AddStream(audioStream, videoStream)
                .AddParameter("-strict -2", ParameterPosition.PostInput)
                .SetOutput(outputPath)
                .Start();
            return conversionResult;
        }

        private static async Task AddQualityToDatabase(EditVideoMessage message, AppDbContext ctx, Submission submission, CancellationToken stoppingToken, VideoSize videoSize)
        {
            string videoLink = message.Output + "FFmpeg_" + videoSize + ".mp4";
            VideoQuality videoQuality = new VideoQuality
            {
                QualityName = videoSize.ToString(),
                Submission = submission,
                SubmissionId = submission.Id,
                QualityVideoLink = videoLink
            };
            submission.VideoQualities.Add(videoQuality);
            await ctx.SaveChangesAsync(stoppingToken);
        }

        private static ConvertOption GetConvertOptionMaxQualityToConvert(IMediaInfo mediaInfo)
        {
            ConvertOption videoSize = ConvertOption.Option240;
            int videoHeight = mediaInfo.VideoStreams.FirstOrDefault().Height;
            if (videoHeight >= 1080)
            {
                videoSize = ConvertOption.Option1080;
                return videoSize;
            }
            else if (videoHeight < 1080 && videoHeight >= 720)
            {
                videoSize = ConvertOption.Option720;
                return videoSize;
            }
            else if (videoHeight < 720 && videoHeight >= 480)
            {
                videoSize = ConvertOption.Option480;
                return videoSize;
            }
            else if (videoHeight < 480 && videoHeight >= 360)
            {
                videoSize = ConvertOption.Option360;
                return videoSize;
            }
            else if (videoHeight < 359)
            {
                videoSize = ConvertOption.Option240;
                return videoSize;
            }
            return videoSize;
        }

        private static void DisplayResultToConsole(IConversionResult conversionResult)
        {
            Console.WriteLine("\nDuration: " + conversionResult.Duration + "\nCommand: " + conversionResult.Arguments + "\n");
        }

        private async Task GetSnapshot(string previewImageName, string inputPath)
        {
            string outputSnapshot = Path.Combine(_env.WebRootPath, previewImageName);
            IConversion conversionSnapshot = FFmpeg.Conversions.New().AddParameter($"-i {inputPath} -ss {TimeSpan.FromSeconds(0)}" +
                                                                                    $" -vframes 1 -vf scale=800x400 {outputSnapshot}");
            IConversionResult resultSnapshot = await conversionSnapshot.Start();
        }

        private static void DeleteSourceFile(string inputPath)
        {
            if (File.Exists(inputPath))
            {
                File.Delete(inputPath);
            }
        }
    }
    public enum ConvertOption
    {
        Option1080 = 1,
        Option720,
        Option480,
        Option360,
        Option240
            
    }
    public class EditVideoMessage
    {
        public int SubmissionId { get; set; }
        public string Input { get; set; }

        public string Output { get; set; }
    }
}
