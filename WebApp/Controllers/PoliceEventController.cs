using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models.PoliceEvent;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceEvents")]
    public class PoliceEventController : Controller
    {

        private readonly IRepository<PoliceEvent> _repository;
        private readonly IReadData<JsonDocument> _apiCaller;
        private readonly string path = "events/";

        #pragma warning disable IDE0290
        public PoliceEventController(IReadData<JsonDocument> apiCaller, IRepository<PoliceEvent> repository)
        {
            _repository = repository;
            _apiCaller = apiCaller;
        }

        [HttpGet]
        [Route("GetAllPoliceEvents")]
        public IActionResult GetAllPoliceEvents()
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAll());
        }

        [HttpGet]
        [Route("GetPoliceEventsByLocation/{location}")]
        public IActionResult GetPoliceEventsByLocation(string location)
        {
            location = location.Trim(); //Remove white spaces 
            location = char.ToUpper(location[0]) + location[1..]; //Capitalize first letter 

            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByLocationName(location));
        }

        [HttpGet]
        [Route("GetPoliceEventsByType/{type}")]
        public IActionResult GetPoliceEventsByDisplayName(string type)
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByType(type));
        }
    }
}
