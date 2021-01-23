using AudioPlayerProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Data
{
    public class PlaylistContext : DbContext
    {
        public PlaylistContext(DbContextOptions<PlaylistContext> options) : base(options)
        {

        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<PlaylistMusic> PlaylistMusics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>().ToTable("Playlist");
            modelBuilder.Entity<Music>().ToTable("Music");
            modelBuilder.Entity<PlaylistMusic>().ToTable("PlaylistMusic");
        }
    }
}
