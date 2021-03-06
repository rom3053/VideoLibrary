﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoLibrary.API.Models;
using VideoLibrary.Data;

namespace VideoLibrary.API.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly AppDbContext _ctx;
            
        public VideosController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Video> All() => _ctx.Videos.ToList();

        [HttpGet("{id}")]
        public Video Get(string id) =>
            _ctx.Videos
            .FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

        [HttpGet("{videoId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForVideo(string videoId) {
            var sub = _ctx.Submissions
             .Single(x => x.VideoId.Equals(videoId, StringComparison.InvariantCultureIgnoreCase));

            _ctx.Entry(sub)
                     .Collection(b => b.VideoQualities)
                     .Load();
            List<VideoQuality> qualities = new List<VideoQuality>();
            foreach (var item in sub.VideoQualities)
            {
                qualities.Add(new VideoQuality
                {
                    Submission = null,
                    SubmissionId = 23,
                    Deleted = false,
                    Id = item.Id,
                    QualityName = item.QualityName,
                    QualityVideoLink = item.QualityVideoLink
                }); 
            }

            
            Submission submissionToSend = new Submission
            {
                Deleted = sub.Deleted,
                Description = sub.Description,
                Id = sub.Id,
                VideoFile = sub.VideoFile,
                VideoId = sub.VideoId,
                VideoProcessed = sub.VideoProcessed,
                VideoQualities = qualities
            };
            List<Submission> submissions = new List<Submission>();
            submissions.Add(submissionToSend);
            return submissions;
}

        [HttpPost]
        public async Task<Video> Create([FromBody] Video video)
        {
            //video.Id = video.Name.Replace(" ", "-").ToLowerInvariant();
            video.Id = Guid.NewGuid().ToString("N");
            _ctx.Add(video);
            await  _ctx.SaveChangesAsync();
            return video;
        }

        [HttpPut]
        public async Task<Video> Update([FromBody] Video video)
        {
            if (string.IsNullOrEmpty(video.Id))
            {
                return null;
            }
            _ctx.Add(video);
            await _ctx.SaveChangesAsync();
            return video;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var video = _ctx.Videos.FirstOrDefault(x => x.Id.Equals(id));
            video.Deleted = true;
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }

}
