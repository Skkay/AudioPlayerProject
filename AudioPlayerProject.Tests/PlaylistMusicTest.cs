using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AudioPlayerProject.Tests
{
    [TestCaseOrderer("AudioPlayerProject.Tests.AlphabeticalOrderer", "AudioPlayerProject.Tests")]
    public class PlaylistMusicTest
    {
        private PlaylistContext context;

        public PlaylistMusicTest()
        {
            var options = new DbContextOptionsBuilder<PlaylistContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AudioPlayerProject;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            this.context = new PlaylistContext(options);
        }

        [Fact]
        public void a_AddToPlaylist()
        {
            string result = null;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    PlaylistMusic pm = new PlaylistMusic { Id = -1, MusicId = -1, PlaylistId = -1 };
                    context.PlaylistMusics.Add(pm);

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.PlaylistMusic ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.PlaylistMusic OFF;");

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
        public void b_RemoveFromPlaylist()
        {
            string result = null;
            try
            {
                PlaylistMusic pm = context.PlaylistMusics.Find(-1);
                context.PlaylistMusics.Remove(pm);
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
