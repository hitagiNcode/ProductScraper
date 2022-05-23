namespace ProductScraper.Utility
{
    public interface IProductTrackScopedProcessingService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}
