using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Models.PoliceStation;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceStation")]
    public class PoliceStationController : Controller
    {
        private readonly IRepository<PoliceStation, ServiceType> _repository;
        private readonly IReadData<JsonDocument> _apiCaller;
        private readonly string path = "policestations/";

        #pragma warning disable IDE0290
        public PoliceStationController(IReadData<JsonDocument> apiCaller, IRepository<PoliceStation, ServiceType> repository)
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
