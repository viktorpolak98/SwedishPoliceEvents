using Microsoft.AspNetCore.Mvc;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("/PoliceStation")]
    public class PoliceStationController : Controller
    {
        private readonly PoliceStationsRepository _repository = new();
        private readonly PoliceAPICaller _apiCaller;
        //TODO: Change to real path
        private readonly string path = "";

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
