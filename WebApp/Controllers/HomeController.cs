using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        public PoliceEventsRepository Repository { get; set; }


        [HttpGet]
        [Route("GetAllPoliceEvents")]
        public IActionResult GetAllPoliceEvents()
        {


            return Ok("stuff");
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


        [HttpGet]
        [Route("GetAllPoliceStations")]
        public IActionResult GetAllPoliceStations()
        {
            return Ok("stuff");
        }

        [HttpGet]
        [Route("GetPoliceStationsByLocation")]
        public IActionResult GetPoliceStationsByLocation()
        {
            return Ok("stuff");
        }

        [HttpGet]
        [Route("GetPoliceStationsByService")]
        public IActionResult GetPoliceStationsByService()
        {
            return Ok("stuff");
        }
    }
}
