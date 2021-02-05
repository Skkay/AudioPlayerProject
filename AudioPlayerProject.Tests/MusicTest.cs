using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AudioPlayerProject.Tests
{
    [TestCaseOrderer("AudioPlayerProject.Tests.AlphabeticalOrderer", "AudioPlayerProject.Tests")]
    public class MusicTest
    {
        private MusicContext context;

        public MusicTest()
        {
            var options = new DbContextOptionsBuilder<MusicContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AudioPlayerProject;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            this.context = new MusicContext(options);
        }

        [Fact]
        public void a_Insert()
        {
            string result = null;
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Music music = new Music { Id = -1, Title = "test_title", Artist = "test_artist", Path = "test_path", Duration = -1 };
                    context.Musics.Add(music);

                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Music ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Music OFF;");

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
    }
}
