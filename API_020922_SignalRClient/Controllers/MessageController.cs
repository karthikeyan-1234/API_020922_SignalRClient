using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace API_020922_SignalRClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        HubConnection connection;

        public MessageController()
        {
            connection = new HubConnectionBuilder().WithUrl("https://localhost:7076/informHub").Build();
            connection.StartAsync().Wait();
            connection.InvokeAsync("JoinGroup", "MyGroup");
        }

        [HttpPost("JoinGroup",Name = "JoinGroup")]
        public IActionResult JoinGroup()
        {
            return Ok();
        }

        [HttpPost("")]


        [HttpPost("StartMonitoring",Name = "StartMonitoring")]
        public IActionResult StartMonitoring()
        {
            return Ok();
        }

        ~MessageController()
        {
            connection.StopAsync().Wait();
            connection.DisposeAsync().GetAwaiter().OnCompleted(() => { });
        }

    }
}
