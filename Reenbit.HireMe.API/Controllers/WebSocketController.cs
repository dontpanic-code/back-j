using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reenbit.HireMe.API.Extensions;
using Reenbit.HireMe.Infrastructure;

namespace Reenbit.HireMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : BaseController
    {
        private readonly SocketManager _socketManager;

        public WebSocketController(SocketManager socketManager, IConfigurationManager configurationManager) 
            : base(configurationManager)
        {
            _socketManager = socketManager;
        }

        public async Task Report(double liquidTemp)
        {
            var reading = new
            {
                Date = DateTime.Now,
                LiquidTemp = liquidTemp
            };

            await _socketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(reading));
        }

        [HttpGet]
        public async Task Generate()
        {
            var rnd = new Random();
            await Report(rnd.Next(23, 35));
            //for (var i = 0; i < 100; i++)
            //{
            //    await Report(rnd.Next(23, 35));
            //    await Task.Delay(5000);
            //}
        }

    }
}