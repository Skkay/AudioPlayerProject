﻿using AudioPlayerProject.Data;
using AudioPlayerProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AudioPlayerProject.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private PlaylistContext context;
        private string uploadsFolderName;

        public PlaylistController(PlaylistContext context)
        {
            this.uploadsFolderName = "uploads";
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
            TempData["ConfirmationResult"] = new string[] { "danger", "Une erreur s'est produite lors de la création de la playlist." };
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                /* Check if a playlist with this name already exists in database */
                bool playlistAlreadyExist = context.Playlists.ToList().Where(p => p.AudioPlayerProjectUserId == userId && p.Name == playlist.Name).Any();
                if (playlistAlreadyExist)
                {
                    TempData["ConfirmationResult"] = new string[] { "danger", "Une playlist de ce nom existe déjà." };
                    return RedirectToAction();
                }

                playlist.AudioPlayerProjectUserId = userId;

                context.Playlists.Add(playlist);
                context.SaveChanges();

                TempData["ConfirmationResult"] = new string[] { "success", "La playlist a été créée avec succès !" };
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
            if (UserOwnsPlaylist(id))
            {
                Playlist playlist = context.Playlists.Find(id);
                ViewBag.Playlist = playlist;
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Update(int playlist_id, string new_playlist_name)
        {
            TempData["ConfirmationResult"] = new string[] { "danger", "Une erreur s'est produite lors du renommage de la playlist." };
            try
            {
                if (UserOwnsPlaylist(playlist_id))
                {
                    Playlist playlist = context.Playlists.Find(playlist_id);
                    playlist.Name = new_playlist_name;

                    context.Playlists.Update(playlist);
                    context.SaveChanges();

                    TempData["ConfirmationResult"] = new string[] { "success", "La playlist a été renommée avec succès !" };
                }
                else
                {
                    TempData["ConfirmationResult"] = new string[] { "danger", "La playlist est introuvable." };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (UserOwnsPlaylist(id))
            {
                Playlist playlist = context.Playlists.Find(id);
                ViewBag.Playlist = playlist;
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(int playlist_id, string confirm_playlist_name)
        {
            TempData["ConfirmationResult"] = new string[] { "danger", "Une erreur s'est produite lors de la suppression de la playlist." };
            try
            {
                if (UserOwnsPlaylist(playlist_id))
                {
                    Playlist playlist = context.Playlists.Find(playlist_id);

                    if (confirm_playlist_name == playlist.Name)
                    {
                        context.Playlists.Remove(playlist);
                        context.SaveChanges();

                        TempData["ConfirmationResult"] = new string[] { "success", "La playlist a été supprimée avec succès !" };
                    }
                    else
                    {
                        TempData["ConfirmationResult"] = new string[] { "warning", "La confirmation est invalide." };
                    }
                }
                else
                {
                    TempData["ConfirmationResult"] = new string[] { "danger", "La playlist est introuvable." };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
        {
            Playlist playlist = context.Playlists.Find(id);
            List<int> musicIds = context.PlaylistMusics.Where(p => p.PlaylistId == id).Select(p => p.MusicId).ToList();
            List<Music> musics = context.Musics.Where(m => musicIds.Contains(m.Id)).ToList();

            ViewBag.Musics = musics;
            ViewBag.Playlist = playlist;
            return View();
        }

        public IActionResult RemoveFromPlaylist(int playlist_id, int music_id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get actual user id
            IEnumerable<Playlist> usersPlaylist = context.Playlists.ToList().Where(p => (p.AudioPlayerProjectUserId == userId) && (p.Id == playlist_id));

            if (usersPlaylist.Any()) // If user owns this playlist
            {
                PlaylistMusic pm = context.PlaylistMusics.Where(pm => (pm.PlaylistId == playlist_id && (pm.MusicId == music_id))).First();
                context.PlaylistMusics.Remove(pm);
                context.SaveChanges();
            }

            return RedirectToAction("Show", new { id = playlist_id });
        }

        public IActionResult Play(int id)
        {
            Playlist playlist = context.Playlists.Find(id);
            if (playlist == null)
            {
                TempData["ConfirmationResult"] = new string[] { "danger", "La playlist est introuvable." };
                return RedirectToAction("Index");
            }

            List<int> musicIds = context.PlaylistMusics.Where(p => p.PlaylistId == id).Select(p => p.MusicId).ToList();
            List<Music> musics = context.Musics.Where(m => musicIds.Contains(m.Id)).ToList();

            ViewBag.Playlist = playlist;
            ViewBag.Musics = musics;
            ViewBag.BaseUploadsPath = this.uploadsFolderName;
            return View();
        }

        private bool UserOwnsPlaylist(int playlist_id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get actual user id
            IEnumerable<Playlist> userPlaylists = context.Playlists.ToList().Where(p => (p.AudioPlayerProjectUserId == userId) && (p.Id == playlist_id));

            return userPlaylists.Any();
        }
    }
}
