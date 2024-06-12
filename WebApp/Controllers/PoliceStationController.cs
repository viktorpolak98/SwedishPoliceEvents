using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceStation")]
    public class PoliceStationController : Controller
    {
        private readonly PoliceStationsRepository _repository;
        private readonly IReadData<JsonDocument> _apiCaller;
        private readonly string path = "policestations/";

        public PoliceStationController(IReadData<JsonDocument> apiCaller, PoliceStationsRepository repository)
        {
            _repository = repository;
            _apiCaller = apiCaller;
        }

        [HttpGet]
        [Route("GetAllPoliceStations")]
        public IActionResult GetAllPoliceStations()
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAll());
        }

        [HttpGet]
        [Route("GetPoliceStationsByLocation")]
        public IActionResult GetPoliceStationsByLocation(string location)
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByLocationName(location));
        }

        [HttpGet]
        [Route("GetPoliceStationsByService")]
        public IActionResult GetPoliceStationsByService(string service)
        {
            if (!_repository.CacheIsFull())
            {
                _repository.CreateValues(_apiCaller.ReadData(path).Result);
            }

            return Ok(_repository.GetAllByDisplayName(service));
        }
    }
}
