using System;
using System.Collections.Generic;

namespace R4R_API.Models
{
    public class getAllRoom
    {

        public Motor room { get; set; } = null!;

        public List<string>? ImgRoom { get; set; }

        public string[] Utilities { get; set; } = null!;
    }
}


