using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.LifeCoaches;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class LifeCoachesController : Controller
    {
        private readonly ILifeCoachService lifeCoaches;

        public LifeCoachesController(ILifeCoachService lifeCoaches)
        {
            this.lifeCoaches = lifeCoaches;
        }

        public IActionResult All([FromQuery] AllLifeCoachesQueryModel query)
        {
            var queryResult = this.lifeCoaches.All(
                 query.CurrentPage,
                 AllLifeCoachesQueryModel.LifeCoachesPerPage);

            query.LifeCoaches = queryResult.LifeCoaches;
            query.TotalLifeCoaches = queryResult.TotalLifeCoaches;

            return View(query);
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

            this.lifeCoaches.Become(
                lifeCoach.FirstName,
                lifeCoach.LastName,
                lifeCoach.ProfilePictureUrl,
                lifeCoach.About,
                userId);

            TempData[GlobalMessageKey] = "Thank you for applying to become a Life Coach! Your application will be reviewed.";

            return RedirectToAction(nameof(All));
        }

        public IActionResult DeleteApplication(int id)
        {
            this.lifeCoaches.DeleteApplication(id);

            return RedirectToAction(nameof(All));
        }
    }
}
