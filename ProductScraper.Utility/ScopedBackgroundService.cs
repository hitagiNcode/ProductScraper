
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProductScraper.Utility
{
    public class ScopedBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScopedBackgroundService> _logger;

        public ScopedBackgroundService(
       IServiceProvider serviceProvider,
       ILogger<ScopedBackgroundService> logger) =>
       (_serviceProvider, _logger) = (serviceProvider, logger);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
            $"{nameof(ScopedBackgroundService)} is running.");
            await DoWorkAsync(stoppingToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"{nameof(ScopedBackgroundService)} is working.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IProductTrackScopedProcessingService scopedProcessingService =
                    scope.ServiceProvider.GetRequiredService<IProductTrackScopedProcessingService>();

                await scopedProcessingService.DoWorkAsync(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"{nameof(ScopedBackgroundService)} is stopping.");

            await base.StopAsync(stoppingToken);
        }

    }
}
