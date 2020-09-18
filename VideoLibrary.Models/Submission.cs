using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.API.Models
{
    public class Submission : BaseModel
    {
        public int VideoId { get; set; }
        public string VideoFile { get; set; }
        public string Description { get; set; }
    }
}
