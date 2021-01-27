using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AudioPlayerProject.Controllers
{
    public class MusicController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private MusicContext contextMusic;
        private PlaylistContext contextPlaylist;
        private string uploadsFolderName;
        private string uploadsFolderPath;

        public MusicController(IHostingEnvironment environment, MusicContext contextMusic, PlaylistContext contextPlaylist)
        {
            hostingEnvironment = environment;
            this.contextMusic = contextMusic;
            this.contextPlaylist = contextPlaylist;

            this.uploadsFolderName = "uploads";
            this.uploadsFolderPath = Path.Combine(hostingEnvironment.WebRootPath, this.uploadsFolderName);
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get actual user id
            ViewBag.MusicList = contextMusic.Musics.ToList();
            ViewBag.PlaylistList = contextPlaylist.Playlists.ToList().Where(p => p.AudioPlayerProjectUserId == userId).ToList();
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
                    
                    contextMusic.Musics.Add(music);
                    contextMusic.SaveChanges();

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
            Music music = contextMusic.Musics.Find(id);
            ViewBag.Music = music;
            ViewBag.BaseUploadsPath = this.uploadsFolderName;
            return View();
        }

        public IActionResult AddToPlaylist(int playlist_id, int music_id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get actual user id
            IEnumerable<Playlist> usersPlaylist = contextPlaylist.Playlists.ToList().Where(p => (p.AudioPlayerProjectUserId == userId) && (p.Id == playlist_id));

            if (usersPlaylist.Any()) // If user owns this playlist
            {
                PlaylistMusic pm = new PlaylistMusic() { MusicId = music_id, PlaylistId = playlist_id };

                contextPlaylist.PlaylistMusics.Add(pm);
                contextPlaylist.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }
    }
}
