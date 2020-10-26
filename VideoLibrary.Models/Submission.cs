using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.API.Models
{
    public class Submission : BaseModel<int>
    {
        public string VideoId { get; set; }
        public string VideoFile { get; set; }
        public bool VideoProcessed { get; set; }
        public string Description { get; set; }
    }
}
