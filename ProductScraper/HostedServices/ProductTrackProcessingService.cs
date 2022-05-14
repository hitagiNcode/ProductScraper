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
                    if (result >= 5)
                    {
                        _logger.LogInformation($"{objProduct.ASIN} More than 5 hours and it isexactly:{result}");
                        
                        Product updatedProduct = await ScrapeFromLink.TrackProductFromUrlAsync(objProduct.URL);
                        CheckDifferencesFromLastSync(objProduct, updatedProduct);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private void CheckDifferencesFromLastSync(Product outdatedProduct, Product updatedProduct)
        {
            if(outdatedProduct.Name != updatedProduct.Name)
            {
                _logger.LogInformation($"Product name change from {outdatedProduct.Name}. New name: {updatedProduct.Name}");
            }
            else
            {
                _logger.LogInformation("Product name is exactly same");
            }
            if(outdatedProduct.Price != updatedProduct.Price)
            {
                _logger.LogInformation($"Product price change from {outdatedProduct.Price}. New price: {updatedProduct.Price}");
            }
            else
            {
                _logger.LogInformation("Product price is exactly same");

            }
            if (outdatedProduct.PictureUri != updatedProduct.PictureUri)
            {
                _logger.LogInformation($"Product pictureURL change from {outdatedProduct.PictureUri}. New picture: {updatedProduct.PictureUri}");
            }
            else
            {
                _logger.LogInformation("Product picture is exactly same");

            }
        }

    }
}
