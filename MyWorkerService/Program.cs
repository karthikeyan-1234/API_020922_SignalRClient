using MyWorkerService;
using Serilog;





IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging => 
    {
        logging.AddSerilog();
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();





await host.RunAsync();
