using HealthyLifestyleTrackingApp.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class TrackerController : Controller
    {
        [Authorize]
        public IActionResult ViewTracked()
        {
            if (this.User.GetId() == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            return View();
        }
    }
}
