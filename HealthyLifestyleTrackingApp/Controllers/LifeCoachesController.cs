using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.LifeCoaches;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class LifeCoachesController : Controller
    {
        private readonly ILifeCoachService lifeCoaches;

        public LifeCoachesController(ILifeCoachService lifeCoaches)
        {
            this.lifeCoaches = lifeCoaches;
        }

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeLifeCoachFormModel lifeCoach)
        {
            var userId = this.User.GetId(); 

            var userIsAlreadyLifeCoach = lifeCoaches.IsLifeCoach(userId);

            if (userIsAlreadyLifeCoach)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(lifeCoach);
            }

            this.lifeCoaches.Create(
                lifeCoach.FirstName,
                lifeCoach.LastName,
                lifeCoach.ProfilePictureUrl,
                lifeCoach.About,
                userId);

            return RedirectToAction("Home", "Index");
        }
    }
}
