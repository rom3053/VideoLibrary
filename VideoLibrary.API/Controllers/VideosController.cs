using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoLibrary.API.Models;

namespace VideoLibrary.API.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly VideoStore _store;
            
        public VideosController(VideoStore store)
        {
            _store = store;
        }

        // /api/videos
        [HttpGet]
        public IActionResult All() => Ok(_store.All);

        // /api/videos/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(_store.All.FirstOrDefault(x => x.Id.Equals(id)));

        //// /api/videos/{id}/submissions?
        //[HttpGet("{id}")]
        //public IActionResult GetSub(int id) => Ok(_store.All.FirstOrDefault(x => x.Id.Equals(id)));

        // /api/videos/
        [HttpPost]
        public IActionResult Create([FromBody] Video video)
        {
            _store.Add(video);
            return Ok();
        }

        // /api/videos/
        [HttpPut]
        public IActionResult Update([FromBody] Video video)
        {
            _store.Add(video);
            return Ok();
        }

        // /api/videos/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}