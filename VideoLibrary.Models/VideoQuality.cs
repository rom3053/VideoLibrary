using System;
using System.Collections.Generic;
using System.Text;


namespace VideoLibrary.API.Models
{
    public class VideoQuality: BaseModel<int>
    {
        public int? SubmissionId { get; set; }
        public Submission Submission { get; set; }
        public string QualityName { get; set; }
        public string QualityVideoLink { get; set; }
    }
}
