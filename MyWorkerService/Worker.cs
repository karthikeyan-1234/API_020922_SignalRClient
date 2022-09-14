using Microsoft.AspNetCore.SignalR.Client;

namespace MyWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        HubConnection connection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7076/informHub").Build();
            connection.StartAsync().Wait();
            connection.KeepAliveInterval = TimeSpan.FromDays(100);
            connection.Closed(() = > { });
            connection.InvokeAsync("JoinGroup", "MyGroup").Wait();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                connection.On<string>("Receive", (res) => { _logger.LogInformation(res); });
            }
        }
    }
}