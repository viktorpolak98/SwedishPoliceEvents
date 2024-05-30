using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceStation")]
    public class PoliceStationController : Controller
    {
        [HttpGet]
        [Route("GetAllPoliceStations")]
        public IActionResult GetAllPoliceStations()
        {
            return Ok("stuff");
        }

        [HttpGet]
        [Route("GetPoliceStationsByLocation")]
        public IActionResult GetPoliceStationsByLocation(string location)
        {
            return Ok("stuff");
        }

        [HttpGet]
        [Route("GetPoliceStationsByService")]
        public IActionResult GetPoliceStationsByService(string service)
        {
            return Ok("stuff");
        }
    }
}
