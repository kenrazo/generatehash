using Infrastructure.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BackgroundWorker
{
    public class ConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConsumerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var rmqService = scope.ServiceProvider.GetRequiredService<IRabbitmqService>();

            rmqService.ConsumeMessage();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }


        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var rmqService = scope.ServiceProvider.GetRequiredService<IRabbitmqService>();
            rmqService.CloseConnection();

            await base.StopAsync(stoppingToken);
        }
    }
}
