using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;

namespace AudioPlayerProject.Tests
{
    [TestCaseOrderer("AudioPlayerProject.Tests.AlphabeticalOrderer", "AudioPlayerProject.Tests")]
    public class PlaylistTest
    {
        private PlaylistContext context;

        public PlaylistTest()
        {
            var options = new DbContextOptionsBuilder<PlaylistContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AudioPlayerProject;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            this.context = new PlaylistContext(options);
        }
        
        [Fact]
        public void a_Insert()
        {
            string result = null;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Playlist playlist = new Playlist { AudioPlayerProjectUserId = "test_id", Id = -1, Name = "test_name" };
                    context.Playlists.Add(playlist);

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Playlist ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Playlist OFF;");

                    transaction.Commit();

                    result = "ok";
                }
            }
            catch (Exception e)
            {
                result = e.ToString();
            }

            Assert.True(result == "ok", result);
        }

        [Fact]
        public void b_Update()
        {
            string result = null;
            try
            {
                Playlist p = context.Playlists.Find(-1);
                p.Name = "new_test_name";

                context.Playlists.Update(p);
                context.SaveChanges();

                Playlist new_p = context.Playlists.Find(-1);
                result = new_p.Name;
            }
            catch (Exception e)
            {
                result = e.ToString();
            }

            Assert.True(result == "new_test_name", result);
        }

        [Fact]
        public void c_Remove()
        {
            string result = null;
            try
            {
                Playlist p = context.Playlists.Find(-1);
                context.Playlists.Remove(p);
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
