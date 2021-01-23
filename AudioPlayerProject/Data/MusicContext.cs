using AudioPlayerProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Data
{
    public class MusicContext : DbContext
    {
        public MusicContext(DbContextOptions<MusicContext> options) : base(options)
        {

        }

        public DbSet<Music> Musics { get; set; }

        protected override void OnModelCreating(ModelBuilder modeBuilder)
        {
            modeBuilder.Entity<Music>().ToTable("Music");
        }
    }
}
