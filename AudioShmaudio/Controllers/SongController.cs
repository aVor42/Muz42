using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; 
using AudioShmaudio.Models;
using AudioShmaudio.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace AudioShmaudio.Controllers
{
    public class SongController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly IFileStorer _fileStorer;

        public SongController(IFileStorer fileStorer, ApplicationContext context)
        {
            _context = context;
            _fileStorer = fileStorer;
        }

        // Принимает код песни
        // Возвращает аудиофайл
        [HttpGet]
        public FileContentResult GetAudio(int audioid)
        {
            var filePath = _context.Songs.Find(audioid).Src;
            var bytes = _fileStorer.GetFile(filePath);
            return File(bytes, "audio/mp3", "file.mp3");
        }

        // Возвращает представление формы для добавления песни
        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public IActionResult CreateSong()
        {
            var artists = _context.Artists.ToList();
            if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
                return PartialView("Partials/_CreateSong", new SongViewModel { Artists = artists});
            return View("Index");
        }

        // Принимает модель приедставления песни
        // Добавляет песню в базу данных
        // Аудиофайл песни сохраняет на в директорию,
        // выбранную в файле конфигурации
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        public async Task<ActionResult> CreateSong(SongViewModel model)
        {
            if(!ModelState.IsValid)
                return PartialView(model);

            var artist = _context.Artists.Find(model.ArtistId);
            var minSec = model.Duration.Split(":");
            var duration = int.Parse(minSec[0]) * 60;
            duration += int.Parse(minSec[1]);

            var src = await _fileStorer.AddFile(model.File);

            var song = new Song
            {
                Name = model.Name,
                Artist = artist,
                Duration = duration,
                Src = src
            };

            _context.Songs.Add(song);
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // Принимает код песни
        // Возвращает модель песни
        [HttpGet]
        public Song GetSong(int id)
        {
            return _context.Songs.Find(id);
        }

        // Принимает поисковый запрос
        // Производит поиск по названиям артистов и
        // названиям песен
        // Возвращает частичное представление со списком найденных песен
        [HttpGet]
        public IActionResult GlobalSearchSongs(string searchValue)
        {
            var param = new SqlParameter("@searchValue", searchValue);
            var songs = _context.Songs.
                                Include(x => x.Artist).
                                Where(song => song.Name.Contains(searchValue) ||
                                song.Artist.Name.Contains(searchValue));

            return PartialView("Partials/_GlobalSearchResult", songs);
        }

        // Принимает код песни
        // Добавляет песню текущему пользователю в плейлист
        [HttpPost]
        public async Task AddSongIntoPlaylist(int songId)
        {
            var currentUser  = this.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _context.Users.
                Include(x => x.Songs).
                FirstOrDefault(x => x.Id == userId);
            var song = _context.Songs.
                Include(x => x.Users).
                FirstOrDefault(x => x.Id == songId);

            user.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        // Принимает код песни
        // Удаляет песню у текущего пользователя из плейлиста
        [HttpDelete]
        public async Task DeleteSongFromPlaylist(int songId)
        {
            try
            {
                var currentUser = this.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = _context.Users.
                    Include(x => x.Songs).
                    FirstOrDefault(x => x.Id == userId);
                var song = _context.Songs.
                    Include(x => x.Users).
                    FirstOrDefault(x => x.Id == songId);

                user.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        // Возвращает частичное представление плейлиста текущего пользователя
        [HttpGet]
        public IActionResult UserPlaylist()
        {
            var currentUser = this.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _context.Users.
                        Include(x => x.Songs).
                        FirstOrDefault(x => x.Id == userId);
            var songs = _context.Songs.
                Include(x => x.Users).
                Include(x => x.Artist).
                Where(x => x.Users.Contains(user)).ToList();

            return PartialView("Partials/_UserPlaylist", songs);
        }

        // Принимает код песни
        // Возвращает true если у текущего
        // пользователя найдена песня в плейлисте
        // иначе false
        [HttpGet]
        public IActionResult PlaylistContainsSong(int songId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Json(false);

                var currentUser = this.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                

                var song = _context.Users.
                            Include(x => x.Songs).
                            FirstOrDefault(x => x.Id == userId).
                            Songs.FirstOrDefault(x => x.Id == songId);

                return Json(song is not null);
            }
            catch(Exception)
            {
                return Json(null);
            }

        }

        // Принимает код песни
        // Увеличивает число прослушиваний песни
        [HttpPost]
        public async Task IncreaseNumberOfListens(int songId)
        {
            if (!User.Identity.IsAuthenticated)
                return;

            var currentUser = this.User;
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userListenedSong = await _context.UserListenedSongs.
                Include(x => x.User).
                Include(x => x.Song).
                FirstOrDefaultAsync(x => x.User.Id == userId && x.Song.Id == songId);

            if (userListenedSong == null)
            {
                var user = _context.Users.Find(userId);
                var song = _context.Songs.Find(songId);
                userListenedSong = new UserListenedSong
                {
                    User = user,
                    Song = song
                };
                _context.UserListenedSongs.Add(userListenedSong);
            }

            ++userListenedSong.Count;

            await _context.SaveChangesAsync();
        }

    }
}
