using ProductScraper.Models;

namespace ProductScraper.HostedServices
{
    public class ProductTrackProcessingService : IScopedProcessingService
    {
        private int _executionCount;
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
                ++_executionCount;

                _logger.LogInformation(
                    "{ServiceName} working, execution count: {Count}",
                    nameof(ProductTrackProcessingService),
                    _executionCount);

                await Task.Delay(10_000, stoppingToken);
            }
        }
    }
}
