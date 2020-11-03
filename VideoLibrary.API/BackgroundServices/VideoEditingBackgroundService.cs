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

                string previewImageName = Guid.NewGuid() + ".png";
                try
                {

                    

                    #region ConvertRegion
                    
                    

                    string convertFileName = message.Output + "FFmpeg" + ".mp4";
                    string outputPath = Path.Combine(_env.WebRootPath, convertFileName);
                    string inputPath = Path.Combine(_env.WebRootPath, message.Input);
                    


                    IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(inputPath);
                    
                    IStream videoStream = mediaInfo.VideoStreams.FirstOrDefault()
                        ?.SetCodec(VideoCodec.h264_nvenc)
                        ?.SetSize(VideoSize.Hd480)
                        ?.SetFramerate(24);
                    
                    IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()
                        ?.SetCodec(AudioCodec.aac);
                    

                    string outputSnapshot = Path.Combine(_env.WebRootPath, previewImageName);
                    IConversion conversionSnapshot =  FFmpeg.Conversions.New().AddParameter($"-i {inputPath} -ss {TimeSpan.FromSeconds(0)}" +
                                                                                            $" -vframes 1 -vf scale=1920x1080 {outputSnapshot}");
                    IConversionResult resultSnapshot = await conversionSnapshot.Start();



                    IConversion conversion = FFmpeg.Conversions.New();
                    conversion.OnProgress += (sender, args) =>
                    {
                        var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                        Console.WriteLine($"[{args.Duration} / {args.TotalLength}] {percent}%");
                    };

                    IConversionResult conversionResult = await conversion
                        .AddParameter("-hwaccel_device 0 -hwaccel cuda", ParameterPosition.PreInput)
                        .AddStream(audioStream, videoStream)
                        .AddParameter("-strict -2", ParameterPosition.PostInput)
                        .SetOutput(outputPath)
                        .Start();

                    Console.WriteLine("\nDuration: " + conversionResult.Duration+"\nCommand: "+conversionResult.Arguments+"\n");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video Processing Failed for {0}", message.Input);
                }
                #endregion

                using (var scope = _serviceProvider.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var submission = ctx.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));
                    var video = ctx.Videos.FirstOrDefault(v => v.Id.Equals(submission.VideoId));
                    video.PreviewImage = previewImageName;
                    
                    submission.VideoFile = message.Output + "FFmpeg" + ".mp4";
                    submission.VideoProcessed = true;

                    await ctx.SaveChangesAsync(stoppingToken);
                }
            }
        }
    }
    public class EditVideoMessage
    {
        public int SubmissionId { get; set; }
        public string Input { get; set; }

        public string Output { get; set; }
    }
}
