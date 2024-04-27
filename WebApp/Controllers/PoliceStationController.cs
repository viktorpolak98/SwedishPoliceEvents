using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PoliceStationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
