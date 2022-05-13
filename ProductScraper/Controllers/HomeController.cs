using Microsoft.AspNetCore.Mvc;
using ProductScraper.Models;
using System.Diagnostics;

namespace ProductScraper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            try
            {
                int proCount = _db.Products.Count();
                ViewData["AmzTrTableCount"] = proCount;
            }
            catch
            {
                ViewData["AmzTrTableCount"] = "??";
            }

            return View();
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