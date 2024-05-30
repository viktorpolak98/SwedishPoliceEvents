using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceEvent")]
    public class PoliceEventController : Controller
    {

        private readonly PoliceEventsRepository _repository = new();
        private readonly PoliceAPICaller _apiCaller;

        public PoliceEventController(HttpClient client)
        {
            _apiCaller = new PoliceAPICaller(client);
        }

        [HttpGet]
        [Route("GetAllPoliceEvents")]
        public IActionResult GetAllPoliceEvents()
        {
            //TODO: Add check if result exists
            _repository.CreateValues(_apiCaller.ReadData("").Result);
            return Ok(_repository.GetAll());
        }

        [HttpGet]
        [Route("GetPoliceEventsByLocation")]
        public IActionResult GetPoliceEventsByLocation(string location)
        {


            return Ok("stuff");
        }

        [HttpGet]
        [Route("GetPoliceEventsByType")]
        public IActionResult GetPoliceEventsByType(string type)
        {


            return Ok("stuff");
        }
    }
}
