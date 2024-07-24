using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System; 
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
            location = location.Trim(); 
            location = char.ToUpper(location[0]) + location[1..]; //Capitalize first letter 

            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByLocationName(location));
        }

        [HttpGet]
        [Route("GetPoliceEventsByType/{type}")]
        public IActionResult GetPoliceEventsByType(string type)
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByType(type));
        }

        [HttpGet]
        [Route("GetPoliceEventsByTypesSortedAscending/{types}")]
        public IActionResult GetPoliceEventsByTypeSortedAscending(string[] types)
        {
            //TODO
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetPoliceEventsByTypesSortedDescending/{types}")]
        public IActionResult GetPoliceEventsByTypeSortedDescending(string[] types)
        {
            //TODO 
            throw new NotImplementedException();
        }

    }
}
