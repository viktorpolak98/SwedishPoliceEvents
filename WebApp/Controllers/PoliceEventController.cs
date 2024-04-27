using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PoliceEventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
