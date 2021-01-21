using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Models.Music
{
    public class Music
    {
        public string MusicTitle { get; set; }
        public IFormFile MusicFile { get; set; }
    }
}
