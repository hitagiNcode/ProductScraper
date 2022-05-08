using Microsoft.AspNetCore.Mvc;

using HtmlAgilityPack;
using ProductScraper.Models;
using System.Text.RegularExpressions;

namespace ProductScraper.Controllers
{
    public class ScrapeController : Controller
    {

        static readonly string testurl = "https://www.amazon.com.tr/dp/B083Y1D8WB/";

        public IActionResult Index()
        {
            //ViewData["Product"] = getProductFromUrl(testurl);
            return View();
        }

        private Product getProductFromUrl(string url)
        {
            Product product = new Product();

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
                product.Price = Regex.Match(proPriceNode, @"\d+").Value;
            }

            string proPictureNode = doc.DocumentNode.SelectSingleNode("//img[@id='landingImage']").Attributes["src"].Value;
            if (proPictureNode != null)
            {
                product.PictureUri = proPictureNode;
            }

            product.ASIN = getBetween(url, "/dp/", "/");

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

        #region OldWithScrapySharp
        /*
        static readonly ScrapingBrowser _scrapBrowser = new();

        private Product getProduct(WebPage web)
        {
            Product product = new Product();

            var doc = new HtmlDocument();
            doc.LoadHtml(web);

            product.URL = testurl;

            string proNameNode = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']").InnerText;
            if (proNameNode != null)
            {
                product.Name = proNameNode;
            }

            string proPriceNode = doc.DocumentNode.SelectSingleNode("//span[@class='a-price-whole']").InnerText;
            if (proPriceNode != null)
            {
                product.Price = proPriceNode;
            }

            string proPictureNode = doc.DocumentNode.SelectSingleNode("//img[@id='landingImage']").Attributes["src"].Value;
            if (proPictureNode != null)
            {
                product.PictureUri = proPictureNode;
            }

            return product;
        }

        private static WebPage GetProductPage(string _url)
        {
            //string url = "https://www.amazon.com.tr/dp/B083Y1D8WB/";

            _scrapBrowser.IgnoreCookies = true;
            _scrapBrowser.Timeout = TimeSpan.FromMinutes(5);
            _scrapBrowser.Headers["User-Agent"] = "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)" +
                " (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            return _scrapBrowser.NavigateToPage(new Uri(_url));
        }
        */
        #endregion

    }

}
