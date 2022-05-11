
namespace ProductScraper.Controllers.Helpers
{
    public class CheckProductDbWorker : ICheckProductDbWorker
    {
        private readonly ILogger<CheckProductDbWorker> _logger;
        private int number = 0;
        public CheckProductDbWorker(ILogger<CheckProductDbWorker> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Interlocked.Increment(ref number);
                _logger.LogInformation($"Worker prints {number}");
                await Task.Delay(TimeSpan.FromSeconds(5000), cancellationToken);
            }
        }
      
    }
}
