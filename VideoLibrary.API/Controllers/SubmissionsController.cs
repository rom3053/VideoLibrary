using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoLibrary.API.BackgroundServices;
using VideoLibrary.API.Models;
using VideoLibrary.Data;

namespace VideoLibrary.API.Controllers
{
    [Route("api/submissions")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public SubmissionsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Submission> All() => _ctx.Submissions
                                                        .Where(x => x.VideoProcessed)
                                                        .ToList();

        [HttpGet("{id}")]
        public Submission Get(int id) => _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id)
                                                                               &&x.VideoProcessed);

        [HttpPost]
        public async Task<Submission> Create(
            [FromBody] Submission submission,
            [FromServices] Channel<EditVideoMessage> channel )
        {
            submission.VideoProcessed = false;
            _ctx.Add(submission);          
            await _ctx.SaveChangesAsync();
            await channel.Writer.WriteAsync(new EditVideoMessage
            {
                SubmissionId = submission.Id,
                Input = submission.VideoFile,
                Output = $"c{DateTime.Now.Ticks}",
            });
            return submission;
        }

        [HttpPut]
        public async Task<Submission> Update([FromBody] Submission submission)
        {
            if (submission.Id == 0)
            {
                return null;
            }
            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();
            return submission;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var submission = _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id));
            if (submission == null)
            {
                return NotFound();
            }
            submission.Deleted = true;
            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }
}