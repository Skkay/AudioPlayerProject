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
        private string uploadsFolderName;
        private string uploadsFolderPath;

        public MusicController(IHostingEnvironment environment, MusicContext context)
        {
            hostingEnvironment = environment;
            this.context = context;

            this.uploadsFolderName = "uploads";
            this.uploadsFolderPath = Path.Combine(hostingEnvironment.WebRootPath, this.uploadsFolderName);
        }

        public IActionResult Index()
        {
            ViewBag.MusicsList = context.Musics.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(Music music)
        {
            ViewData["UploadMusicResult"] = "Failure";
            if (music.File != null)
            {
                try
                {
                    string extension = Path.GetExtension(music.File.FileName);
                    string fileName = music.Title + (string.IsNullOrWhiteSpace(music.Artist) ? "" : " - " + music.Artist) + extension;
                    
                    string filePath = Path.Combine(this.uploadsFolderPath, fileName);

                    music.Path = fileName;
                    music.File.CopyTo(new FileStream(filePath, FileMode.Create));
                    
                    context.Musics.Add(music);
                    context.SaveChanges();

                    ViewData["UploadMusicResult"] = "Success";
                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
            }
            return RedirectToAction();
        }

        public IActionResult Play(int id)
        {
            Music music = context.Musics.Find(id);
            ViewBag.Music = music;
            ViewBag.BaseUploadsPath = this.uploadsFolderName;
            return View();
        }
    }
}
