using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductScarper.DataAccess.Repository.IRepository;
using ProductScraper.Domain;
using ProductScraper.Utility;
using System.Security.Claims;

namespace ProductScraper.Controllers
{
    public class AmzTrProductTrackController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmzTrProductTrackController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET
        [Authorize]
        public IActionResult Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll(u => u.FollowingUsers.Any(m => m.UserId == currentUserId));
            return View(objProductList);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Product _link)
        {
            if (!ScrapeFromLink.CheckProductLink(_link.URL))
            {
                ModelState.AddModelError("URL", "Geçersiz link! Örnek: https://www.amazon.com.tr/dp/B083Y1D8WB/");
                return View();
            }
            //Check if the product ASIN is already in database
            string proAsin = ScrapeFromLink.getAsinFromUrl(_link.URL, "/dp/");
            Product existingProduct = _unitOfWork.Product.GetFirstOrDefault(u => u.ASIN == proAsin);
            if (existingProduct == null)
            {
                Product newProduct;
                try
                {
                    newProduct = ScrapeFromLink.getProductFromUrl(_link.URL);
                    if (string.IsNullOrEmpty(newProduct.Name))
                    {
                        ModelState.AddModelError("URL", "Ürünün İsim bilgisi yok, linki kontrol ediniz! Örnek: https://www.amazon.com.tr/dp/B083Y1D8WB/");
                        return View();
                    }
                    if (string.IsNullOrEmpty(newProduct.ASIN))
                    {
                        ModelState.AddModelError("URL", "Ürün Asin bilgisi yok, linki kontrol ediniz! Örnek: https://www.amazon.com.tr/dp/B083Y1D8WB/");
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("URL", "Ürün bilgilerini çekemedik" + ex);
                    return View();
                }
                return View(newProduct);
            }
            else
            {
                return View(existingProduct);
            }
        }

        //POST
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddProducts(Product _product)
        {
            Product checkProduct = _unitOfWork.Product.GetFirstOrDefault(u => u.ASIN == _product.ASIN);
            TrackingUser newTrackingUser = new TrackingUser();
            newTrackingUser.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (checkProduct == null)
            {
                if (ModelState.IsValid)
                {
                    newTrackingUser.Product = _product;
                    _product.FollowingUsers = new List<TrackingUser>();
                    _product.FollowingUsers.Add(newTrackingUser);
                    _unitOfWork.Product.Add(_product);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                newTrackingUser.Product = checkProduct;
                if (checkProduct.FollowingUsers != null)
                    checkProduct.FollowingUsers.Add(newTrackingUser);
                else
                {
                    checkProduct.FollowingUsers = new List<TrackingUser>();
                    checkProduct.FollowingUsers.Add(newTrackingUser);
                }
                _unitOfWork.Product.Update(checkProduct);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

    }
}
