using HtmlAgilityPack;
using ProductScraper.Models;
using System.Text.RegularExpressions;

namespace ProductScraper.Controllers.Helpers
{
    public class ScrapeFromLink
    {
        public static async Task<Product> TrackProductFromUrlAsync(string url, CancellationToken stoppingToken)
        {
            Product product = new Product();

            if (!CheckProductLink(url))
            {
                return await Task.FromResult(product);
            }

            var web = new HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(url, stoppingToken);

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

            return await Task.FromResult(product);
        }

        public static bool CheckProductLink(string _link)
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

        public static Product getProductFromUrl(string url)
        {
            Product product = new Product();

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

            product.ASIN = getAsinFromUrl(url, "/dp/");

            string shortUrl = "https://www.amazon.com.tr/dp/" + product.ASIN + "/";
            product.URL = shortUrl;

            return product;
        }

        private static string getAsinFromUrl(string strSource, string strStart)
        {
            if (strSource.Contains(strStart))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = 10;
                return strSource.Substring(Start, End);
            }
            return "";
        }

        /* Eski GetBetween
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
        }*/
    }
}
