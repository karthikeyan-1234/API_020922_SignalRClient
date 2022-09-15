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
            connection.KeepAliveInterval = TimeSpan.FromSeconds(10);
            connection.Closed += Connection_Closed;
            connection.On<string>("Receive", (res) => { _logger.LogInformation(res); });
            connection.InvokeAsync("JoinGroup", "MyGroup").Wait();
        }
        

        private Task Connection_Closed(Exception? arg)
        {
            connection.StopAsync().Wait();
            _logger.LogInformation("SignalR connection closed at: {time}. Attempting to restart connection..", DateTimeOffset.Now);
            connection.StartAsync().Wait();
            connection.InvokeAsync("JoinGroup", "MyGroup").Wait();
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}