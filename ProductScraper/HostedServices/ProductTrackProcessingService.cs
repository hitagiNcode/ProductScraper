using ProductScraper.Controllers.Helpers;
using ProductScraper.Models;

namespace ProductScraper.HostedServices
{
    public class ProductTrackProcessingService : IScopedProcessingService
    {

        private readonly ILogger<ProductTrackProcessingService> _logger;
        private readonly AppDbContext _db;

        public ProductTrackProcessingService(AppDbContext db, ILogger<ProductTrackProcessingService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                IEnumerable<Product> objProductList = _db.Products;

                foreach (Product objProduct in objProductList)
                {

                    var result = (DateTime.UtcNow - objProduct.LastSyncTime).TotalHours;
                    if (result >= 3)
                    {
                        _logger.LogInformation($"{objProduct.ASIN} More than 3 hours and it isexactly:{result}");

                        Product updatedProduct = await ScrapeFromLink.TrackProductFromUrlAsync(objProduct.URL, stoppingToken);
                        objProduct.LastSyncTime = DateTime.Now;
                        await CheckDifferencesFromLastSync(objProduct, updatedProduct, stoppingToken);
                    }
                    
                }
                await _db.SaveChangesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task CheckDifferencesFromLastSync(Product outdatedProduct, Product updatedProduct, CancellationToken stoppingToken)
        {
            bool shouldUpdate = false;

            if (string.IsNullOrEmpty(updatedProduct.Name))
            {
                _db.Add(createProductChange(outdatedProduct.ASIN, "Ürün Url Bozulması", outdatedProduct.URL));
                _logger.LogInformation($"Product url bozulması  name yok eski: {outdatedProduct.Name}. yeni: {updatedProduct.Name}...");
                return;
            }

            if (outdatedProduct.Name != updatedProduct.Name && !string.IsNullOrEmpty(updatedProduct.Name))
            {
                _logger.LogInformation($"Product name change from {outdatedProduct.Name}. New name: {updatedProduct.Name}");
                _db.Add(createProductChange(outdatedProduct.ASIN, "Ürün İsmi", outdatedProduct.Name));
                outdatedProduct.Name = updatedProduct.Name;
                shouldUpdate = true;
            }
            else
            {
                
                _logger.LogInformation("Product name is exactly same");
            }
            if (outdatedProduct.Price != updatedProduct.Price && updatedProduct.Price.HasValue)
            {
                _logger.LogInformation($"Product price change from {outdatedProduct.Price}. New price: {updatedProduct.Price}");
                _db.Add(createProductChange(outdatedProduct.ASIN, "Ürün Fiyatı", outdatedProduct.Price.ToString()));
                outdatedProduct.Price = updatedProduct.Price;
                shouldUpdate = true;

            }
            else
            {
                _logger.LogInformation("Product price is exactly same");

            }
            if (outdatedProduct.PictureUri != updatedProduct.PictureUri && !string.IsNullOrEmpty(updatedProduct.PictureUri))
            {
                _logger.LogInformation($"Product pictureURL change from {outdatedProduct.PictureUri}. New picture: {updatedProduct.PictureUri}");
                _db.Add(createProductChange(outdatedProduct.ASIN, "Ürün Resmi", outdatedProduct.PictureUri));
                outdatedProduct.PictureUri = updatedProduct.PictureUri;
                shouldUpdate = true;
            }
            else
            {
                _logger.LogInformation("Product picture is exactly same");

            }

            if (shouldUpdate)
            {
                _db.Update(outdatedProduct);
            }
            else
            {
                await Task.Delay(0, stoppingToken);
            }

        }

        private ProductChange createProductChange(string _asin, string _changedvar, string _change)
        {
            ProductChange newChange = new ProductChange();
            newChange.ASIN = _asin;
            newChange.ChangedVar = _changedvar;
            newChange.ChangeValue = _change;

            return newChange;
        }

    }
}
