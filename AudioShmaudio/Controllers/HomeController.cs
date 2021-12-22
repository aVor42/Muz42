using AudioShmaudio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AudioShmaudio.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        // Возвращает основное представление,
        // в которое вставляются частичные представления
        public async Task<IActionResult> Index()
        {
            var userListenedSongs = 
                await _context.UserListenedSongs.
                Include(x => x.Song).
                Include(x => x.Song.Artist).
                ToListAsync();

            var songs = (from song in userListenedSongs
                        group song by song.Song into groups
                        select new { Song = groups.Key, Count = groups.Sum(x => x.Count)} into listenedCounts
                        orderby listenedCounts.Count descending
                        select listenedCounts.Song).Take(5);

            return View(songs);
        }

        //Возращает представение ошибки
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
