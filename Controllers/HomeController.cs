using System.Diagnostics;
using BookShop.Models;
using BookShop.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homerepository;   
        public HomeController(ILogger<HomeController> logger, IHomeRepository homerepository)
        {
            _homerepository = homerepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm="",int genreId=0 )
        {
           
            IEnumerable<Book> books = await _homerepository.DisplayBooks( sterm,genreId);
            IEnumerable<Genre> genres = await _homerepository.Genres();
            BookDisplay bookDisplay = new BookDisplay()
            {
                Books = books,
                Genres = genres,
                STerm = sterm,
                GenreId = genreId
            };
            return View(bookDisplay);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
