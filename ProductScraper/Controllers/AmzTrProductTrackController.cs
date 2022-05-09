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
            ProductLink newProduct = getProductFromUrl(_link.URL);
            return View(newProduct);
        }

        static readonly string testurl = "https://www.amazon.com.tr/dp/B083Y1D8WB/";

        private ProductLink getProductFromUrl(string url)
        {
            ProductLink product = new ProductLink();

            var web = new HtmlWeb();
            var doc = web.Load(url);

            string proNameNode = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']").InnerText;
            if (proNameNode != null)
            {
                product.Name = proNameNode;
            }

            string proPriceNode = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']").InnerText;
            if (proPriceNode != null)
            {
                string strPrice = Regex.Match(proPriceNode, @"\d+").Value;
                if (Int32.TryParse(strPrice, out int numValue))
                {
                    product.Price = numValue;
                }
                else
                {
                    product.Price = 0;
                }
            }

            string proPictureNode = doc.DocumentNode.SelectSingleNode("//img[@id='landingImage']").Attributes["src"].Value;
            if (proPictureNode != null)
            {
                product.PictureUri = proPictureNode;
            }

            product.ASIN = getBetween(url, "/dp/", "/");

            string shortUrl = "https://www.amazon.com.tr/dp/"+product.ASIN+"/";
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
