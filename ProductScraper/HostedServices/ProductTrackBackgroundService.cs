
using ProductScraper.Controllers.Helpers;

namespace ProductScraper.HostedServices
{
    public class ProductTrackBackgroundService : BackgroundService
    {
        private readonly ILogger<ProductTrackBackgroundService> _logger;
        private readonly ICheckProductDbWorker _worker;

        public ProductTrackBackgroundService(ILogger<ProductTrackBackgroundService> logger, ICheckProductDbWorker worker)
        {
            _logger = logger;
            _worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _worker.DoWork(stoppingToken);
        }
    }
}
