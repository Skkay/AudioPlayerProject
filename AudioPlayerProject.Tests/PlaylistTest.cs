using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;

namespace AudioPlayerProject.Tests
{
    public class PlaylistTest
    {
        private PlaylistContext context;
        private Playlist playlist;

        public PlaylistTest()
        {
            var options = new DbContextOptionsBuilder<PlaylistContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AudioPlayerProject;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            this.context = new PlaylistContext(options);
            this.playlist = new Playlist() { AudioPlayerProjectUserId = "test_id", Name = "test_name" };
        }
        
        [Fact]
        public void Insert()
        {
            string result = null;
            try
            {
                context.Playlists.Add(this.playlist);
                context.SaveChanges();
                result = "ok";
            }
            catch (Exception e)
            {
                result = e.ToString();
            }

            Assert.True(result == "ok", result);
        }
    }
}
