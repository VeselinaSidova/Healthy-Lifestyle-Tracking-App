using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.LifeCoaches;


namespace HealthyLifestyleTrackingApp.Controllers
{
    public class LifeCoachesController : Controller
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public LifeCoachesController(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeLifeCoachFormModel lifeCoach)
        {
            var userId = this.User.GetId(); 

            var userIsAlreadyLifeCoach = this.data
                .LifeCoaches
                .Any(c => c.UserId == userId);

            if (userIsAlreadyLifeCoach)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(lifeCoach);
            }

            var lifeCoachData = new LifeCoach
            {
                FirstName = lifeCoach.FirstName,
                LastName = lifeCoach.LastName,
                ProfilePictureUrl = lifeCoach.ProfilePictureUrl,
                UserId = userId
            };

            this.data.LifeCoaches.Add(lifeCoachData);
            this.data.SaveChanges();

            return RedirectToAction("All", "Exercises");
        }
    }
}
