using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoLibrary.API.Models
{
    public class Video : BaseModel
    {

        public string Name { get; set; }
        public string VideoFile { get; set; }

 
    }
}
