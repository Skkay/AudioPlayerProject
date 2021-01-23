using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Models
{
    public class PlaylistMusic
    {
        public int Id { get; set; }
        public int MusicId { get; set; }
        public int PlaylistId { get; set; }

        public Music Music { get; set; }
        public Playlist Playlist { get; set; }
    }
}
