using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using static HealthyLifestyleTrackingApp.Areas.Admin.AdminConstants;
using static HealthyLifestyleTrackingApp.WebConstants;


namespace HealthyLifestyleTrackingApp.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdminConstants.AdministratorRoleName)]
    public class LifeCoachesController : Controller
    {
        private readonly ILifeCoachService lifeCoaches;

        public LifeCoachesController(ILifeCoachService lifeCoaches)
        {
            this.lifeCoaches = lifeCoaches;
        }

        public IActionResult All() 
        {
            var lifeCoaches = (this.lifeCoaches.All(approvedOnly: false).LifeCoaches);
            return View(lifeCoaches);
        }

        public IActionResult ApproveForLifeCoach(int id)
        {
            this.lifeCoaches.ApproveForLifeCoach(id);

            TempData[GlobalMessageKey] = "Life Coach application was approved. Please inform user.";

            return RedirectToAction(nameof(All));
        }
    }
}
