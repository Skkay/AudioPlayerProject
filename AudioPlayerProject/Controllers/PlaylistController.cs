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
    }
}
