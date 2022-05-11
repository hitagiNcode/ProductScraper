
namespace ProductScraper.Controllers.Helpers
{
    public interface ICheckProductDbWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}