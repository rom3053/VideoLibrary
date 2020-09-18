using System;
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
        public Video Get(int id) => _ctx.Videos.FirstOrDefault(x => x.Id.Equals(id));

        [HttpGet("{videoId}/submissions")]
        public IEnumerable<Submission> ListSubmissionsForVideo(int videoId) =>
            _ctx.Submissions.Where(x => x.VideoId.Equals(videoId)).ToList();

        [HttpPost]
        public async Task<Video> Create([FromBody] Video video)
        {
            _ctx.Add(video);
            await  _ctx.SaveChangesAsync();
            return video;
        }

        [HttpPut]
        public async Task<Video> Update([FromBody] Video video)
        {
            if (video.Id == 0)
            {
                return null;
            }
            _ctx.Add(video);
            await _ctx.SaveChangesAsync();
            return video;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var video = _ctx.Videos.FirstOrDefault(x => x.Id.Equals(id));
            video.Deleted = true;
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }

}