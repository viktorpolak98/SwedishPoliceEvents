using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public PoliceEventsRepository Repository { get; set; }


        public IActionResult Index()
        {
            return View();
        }
    }
}
