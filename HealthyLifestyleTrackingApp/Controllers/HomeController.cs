using System.Diagnostics;
using HealthyLifestyleTrackingApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Error() => View();
    }
}
