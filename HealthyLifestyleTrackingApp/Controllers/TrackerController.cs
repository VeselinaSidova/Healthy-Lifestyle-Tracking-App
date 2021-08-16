using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class TrackerController : Controller
    {
        [Authorize]
        public IActionResult ViewTracked()
        {
            return View();
        }
    }
}
