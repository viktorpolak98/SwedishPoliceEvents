using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceEvent")]
    public class PoliceEventController : Controller
    {

        private readonly PoliceEventsRepository _repository = new();
        private readonly PoliceAPICaller _apiCaller;
        //TODO: Change to real path
        private readonly string path = "";

        public PoliceEventController(HttpClient client)
        {
            _apiCaller = new PoliceAPICaller(client);
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
        [Route("GetPoliceEventsByLocation?city={location}")]
        public IActionResult GetPoliceEventsByLocation(string location)
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByLocationName(location));
        }

        [HttpGet]
        [Route("GetPoliceEventsByType?type={displayName}")]
        public IActionResult GetPoliceEventsByDisplayName(string displayName)
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByDisplayName(displayName));
        }

        [HttpGet]
        [Route("GetTypeLeaderboard")]
        public IActionResult GetTypeLeaderboard()
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.Leaderboard.NumberOfEventsDict);
        }

        [HttpGet]
        [Route("GetLocationLeaderboard")]
        public IActionResult GetLocationLeaderboard()
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.Leaderboard.NumberOfEventsLocationDict);
        }
    }
}
