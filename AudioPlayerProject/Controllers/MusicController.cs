using AudioPlayerProject.Data;
using AudioPlayerProject.Models.Music;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioPlayerProject.Controllers
{
    public class MusicController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private MusicContext context;
        public MusicController(IHostingEnvironment environment, MusicContext context)
        {
            hostingEnvironment = environment;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(new Music());
        }

        [HttpPost]
        public IActionResult Index(Music music)
        {
            if (music.MusicFile != null)
            {
                var fileName = Path.GetFileName(music.MusicFile.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, fileName);

                music.MusicFile.CopyTo(new FileStream(filePath, FileMode.Create));
                Console.WriteLine("Adding to db");
                try
                {
                    context.Musics.Add(music);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine("Added to db");

            }
            return View();
        }
    }
}
