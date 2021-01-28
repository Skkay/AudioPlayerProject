using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AudioPlayerProject.Controllers
{
    public class PlaylistController : Controller
    {
        private PlaylistContext context;

        public PlaylistController(PlaylistContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.PlaylistList = context.Playlists.Where(p => p.AudioPlayerProjectUserId == userId).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Index(Playlist playlist)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                playlist.AudioPlayerProjectUserId = userId;

                context.Playlists.Add(playlist);
                context.SaveChanges();

                ModelState.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return RedirectToAction();
        }

        public IActionResult Update(int id)
        {
            Playlist playlist = context.Playlists.Find(id);
            ViewBag.Playlist = playlist;
            return View();
        }

        [HttpPost]
        public IActionResult Update(int playlist_id, string new_playlist_name)
        {
            Playlist playlist = context.Playlists.Find(playlist_id);
            playlist.Name = new_playlist_name;

            context.Playlists.Update(playlist);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Playlist playlist = context.Playlists.Find(id);
            ViewBag.Playlist = playlist;
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int playlist_id, string confirm_playlist_name)
        {
            Playlist playlist = context.Playlists.Find(playlist_id);

            if (confirm_playlist_name == playlist.Name)
            {
                context.Playlists.Remove(playlist);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
        {
            Playlist playlist = context.Playlists.Find(id);
            List<int> musicIds = context.PlaylistMusics.Where(p => p.PlaylistId == id).Select(p => p.Id).ToList();
            List<Music> musics = context.Musics.Where(m => musicIds.Contains(m.Id)).ToList();

            ViewBag.Musics = musics;
            ViewBag.Playlist = playlist;
            return View();
        }
    }
}
