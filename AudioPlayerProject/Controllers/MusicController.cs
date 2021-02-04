using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AudioPlayerProject.Controllers
{
    [Authorize]
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
            TempData["UploadMusicResult"] = "Failure";
            if (music.File != null)
            {
                try
                {
                    string extension = Path.GetExtension(music.File.FileName);
                    string fileName = music.Title + (string.IsNullOrWhiteSpace(music.Artist) ? "" : " - " + music.Artist) + extension;
                    string filePath = Path.Combine(this.uploadsFolderPath, fileName);
                    
                    music.Path = fileName;

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    music.File.CopyTo(fs);
                    fs.Close();

                    Mp3FileReader reader = new Mp3FileReader(filePath);
                    music.Duration = (int)Math.Round(reader.TotalTime.TotalSeconds);
                    reader.Close();

                    contextMusic.Musics.Add(music);
                    contextMusic.SaveChanges();

                    TempData["UploadMusicResult"] = "Success";
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
            if (UserOwnsPlaylist(playlist_id)) // If user owns this playlist
            {
                IEnumerable<PlaylistMusic> listPM = contextPlaylist.PlaylistMusics.ToList().Where(pm => (pm.PlaylistId == playlist_id) && (pm.MusicId == music_id));
                if (!listPM.Any()) // Check if music_id isn't already in playlist_id
                {
                    PlaylistMusic pm = new PlaylistMusic() { MusicId = music_id, PlaylistId = playlist_id };

                    contextPlaylist.PlaylistMusics.Add(pm);
                    contextPlaylist.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        private bool UserOwnsPlaylist(int playlist_id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get actual user id
            IEnumerable<Playlist> userPlaylists = contextPlaylist.Playlists.ToList().Where(p => (p.AudioPlayerProjectUserId == userId) && (p.Id == playlist_id));

            return userPlaylists.Any();
        }
    }
}
