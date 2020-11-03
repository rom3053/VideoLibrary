using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoLibrary.API.Models
{
    public class Video : BaseModel<string>
    {

        public string Name { get; set; }
        public string PreviewImage { get; set; }

 
    }
}
