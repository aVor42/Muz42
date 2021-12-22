using Microsoft.AspNetCore.Mvc;
using AudioShmaudio.Models;
using AudioShmaudio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace AudioShmaudio.Controllers
{
    public class ArtistController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly IFileStorer _fileStorer;

        public ArtistController(ApplicationContext context, IFileStorer fileStorer)
        {
            _context = context;
            _fileStorer = fileStorer;
        }

        // Принимает код артиста
        // Возвращает представление страницы артиста
        [HttpGet]
        public IActionResult ArtistPage(int id)
        {
            if (Request.Headers["x-requested-with"] != "XMLHttpRequest")
                return View("Index");

            var artist = _context.Artists.
                Include(x => x.Songs).
                FirstOrDefault(x => x.Id == id);
            var songs = artist.Songs;

            var viewModel = new ArtistViewModel
            {
                Id = artist.Id,
                Name = artist.Name,
                Description = artist.Description,
                Songs = songs
            };

            return PartialView("Partials/_ArtistPage", viewModel);
        }

        // Принимает код артиста
        // Возвращает фотографию артиста
        [HttpGet]
        public FileContentResult ArtistPhoto(int id)
        {
            var artist = _context.Artists.Find(id);
            var bytes = artist is not null ? 
                artist.Photo :
                _fileStorer.GetDefaultFile("img/default_artist.png");
            return File(bytes, "image/jpeg", "artistPhoto.jpg");
        }

        // Возвращает представление формы для создания артиста
        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public IActionResult CreateArtist()
        {
            if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
                return PartialView("Partials/_CreateArtist");
            return View("Index");
        }

        // Принимает модель представления данных артиста
        // Добавляет артиста в базу данных
        [HttpPost]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> CreateArtist(ArtistViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            var artist = new Artist
            {
                Name = model.Name,
                Description = model.Description,
                Photo = await model.Photo.GetBytes()
            };

            _context.Add(artist);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
