using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.API.Models
{
    public abstract class BaseModel<TKey>
    {
        public TKey Id { get; set; }
        public bool Deleted { get; set; }
    }
}
