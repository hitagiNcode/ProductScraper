using Microsoft.AspNetCore.Mvc;
using ProductScraper.Models;

namespace ProductScraper.Controllers
{
    public class AmzTrProductTrackController : Controller
    {
        private readonly AppDbContext _db;
        public AmzTrProductTrackController(AppDbContext db)
        {
            _db = db;  
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Products;
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
