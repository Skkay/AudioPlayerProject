using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Models.Music
{
    public class Music
    {
        [Key]
        public string MusicTitle { get; set; }

        [NotMapped]
        public IFormFile MusicFile { get; set; }
        public string MusicPath { get; set; }
    }
}
