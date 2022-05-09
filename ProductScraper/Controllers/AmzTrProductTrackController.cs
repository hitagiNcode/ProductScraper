using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using ProductScraper.Models;
using System.Text.RegularExpressions;

namespace ProductScraper.Controllers
{
    public class AmzTrProductTrackController : Controller
    {
        private readonly AppDbContext _db;
        
        public AmzTrProductTrackController(AppDbContext db)
        {
            _db = db;
        }

        //GET
        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _db.Products;
            return View(objProductList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(ProductLink _link)
        {

            if (!CheckProductLink(_link.URL))
            {
                ModelState.AddModelError("URL", "Geçersiz link! Örnek: https://www.amazon.com.tr/dp/B083Y1D8WB/");
                return View();
            }

            ProductLink newProduct;
            try
            {
                newProduct = getProductFromUrl(_link.URL);
                if(string.IsNullOrEmpty(newProduct.Name))
                {
                    ModelState.AddModelError("URL", "Ürünün İsim bilgisi yok, linki kontrol ediniz! Örnek: https://www.amazon.com.tr/dp/B083Y1D8WB/");
                    return View();
                }
                if ( string.IsNullOrEmpty(newProduct.ASIN))
                {
                    ModelState.AddModelError("URL", "Ürün Asin bilgisi yok, linki kontrol ediniz! Örnek: https://www.amazon.com.tr/dp/B083Y1D8WB/");
                    return View();
                }
            }
            catch (Exception ex) {
                ModelState.AddModelError("URL", "Ürün bilgilerini çekemedik" +ex);
                return View();
            }

            return View(newProduct);

        }

        //POST
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddProducts()
        {

            return RedirectToAction("Index");
        }

        private bool CheckProductLink(string _link)
        {
            if (_link == null)
            {
                return false;
            }
            if (_link.Contains("https://www.amazon.com.tr/") && _link.Contains("/dp/"))
            {
                return true;
            }
            else if (_link.Contains("https://amazon.com.tr/") && _link.Contains("/dp/"))
            {
                return true;
            }

            return false;
        }

        //static readonly string testurl = "https://www.amazon.com.tr/dp/B083Y1D8WB/";

        private ProductLink getProductFromUrl(string url)
        {
            ProductLink product = new ProductLink();

            var web = new HtmlWeb();
            var doc = web.Load(url);

            HtmlNode proNameNode = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']");
            if (proNameNode != null)
            {
                product.Name = proNameNode.InnerText;
            }

            HtmlNode proPriceNode = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']");
            if (proPriceNode != null)
            {
                string strPrice = Regex.Match(proPriceNode.InnerText, @"\d+").Value;
                if (Int32.TryParse(strPrice, out int numValue))
                {
                    product.Price = numValue;
                }
                else
                {
                    product.Price = 0;
                }
            }

            HtmlNode proPictureNode = doc.DocumentNode.SelectSingleNode("//img[@id='landingImage']");
            if (proPictureNode != null)
            {
                product.PictureUri = proPictureNode.Attributes["src"].Value;
            }

            string urlAsin;
            try
            {
                urlAsin = getBetween(url, "/dp/", "/");
            }
            catch
            {
                urlAsin = getBetween(url, "/dp/", "?");
            }
            
            product.ASIN = urlAsin;

            string shortUrl = "https://www.amazon.com.tr/dp/" + product.ASIN + "/";
            product.URL = shortUrl;

            return product;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            return "";
        }
    }
}
