using HealthyLifestyleTrackingApp.Models.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class TrackerController : Controller
    {
        public IActionResult ViewTracked()
          => View();
    }
}
