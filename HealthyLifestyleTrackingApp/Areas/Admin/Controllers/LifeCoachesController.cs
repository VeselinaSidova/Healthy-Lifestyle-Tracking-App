using Microsoft.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using static HealthyLifestyleTrackingApp.Areas.Admin.AdminConstants;

namespace HealthyLifestyleTrackingApp.Areas.Admin.Controllers
{
    [Area(AreaName)]
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

            return RedirectToAction(nameof(All));
        }
        
        public IActionResult DeleteApplication(int id)
        {
            this.lifeCoaches.DeleteApplication(id);

            return RedirectToAction(nameof(All));
        }
    }
}
