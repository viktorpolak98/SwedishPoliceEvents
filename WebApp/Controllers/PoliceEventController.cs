using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using WebApp.Models.Shared;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceEvent")]
    public class PoliceEventController : Controller
    {

        private readonly PoliceEventsRepository _repository = new();
        private readonly IReadData<JsonDocument> _apiCaller;
        private readonly string path = "events/";

        public PoliceEventController(IReadData<JsonDocument> apiCaller)
        {
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
            string localPath = $"{path}?locationname={location}";

            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(localPath).Result);
            }

            return Ok(_repository.GetAllByLocationName(location));
        }

        [HttpGet]
        [Route("GetPoliceEventsByType/{type}")]
        public IActionResult GetPoliceEventsByDisplayName(string type)
        {
            string localPath = $"{path}?type={type}";

            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(localPath).Result);
            }

            return Ok(_repository.GetAllByDisplayName(type));
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
