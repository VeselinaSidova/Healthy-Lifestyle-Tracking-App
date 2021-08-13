using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Exercises;
using HealthyLifestyleTrackingApp.Services.Exercises;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IExerciseService exercises;
        private readonly ILifeCoachService lifeCoaches;

        public ExercisesController(IExerciseService exercises, ILifeCoachService lifeCoaches)
        {
            this.exercises = exercises;
            this.lifeCoaches = lifeCoaches;
        }

        public IActionResult All([FromQuery] AllExercisesQueryModel query)
        {
            var queryResult = this.exercises.All(
                 query.Category,
                 query.SearchTerm,
                 query.Sorting,
                 query.CurrentPage,
                 AllExercisesQueryModel.ExercisesPerPage);

            var exerciseCategories = this.exercises.GetExerciseCategories().Select(c => c.Name).ToList();

            query.Categories = exerciseCategories;
            query.Exercises = queryResult.Exercises;
            query.TotalExercises = queryResult.TotalExercises;

            return View(query);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!this.lifeCoaches.IsLifeCoach(this.User.GetId()))
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            return View(new CreateExerciseFormModel
            {
                ExerciseCategories = this.exercises.GetExerciseCategories(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateExerciseFormModel exercise)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0)
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!this.exercises.ExerciseCategoryExists(exercise.ExerciseCategoryId))
            {
                this.ModelState.AddModelError(nameof(exercise.ExerciseCategoryId), "Exercise category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                exercise.ExerciseCategories = this.exercises.GetExerciseCategories();
                return View(exercise);
            }

            this.exercises.Create(
                exercise.Name,
                exercise.CaloriesPerHour,
                exercise.ImageUrl,
                exercise.ExerciseCategoryId);

            TempData[GlobalMessageKey] = "Exercise was added and is awaiting approval!";

            return RedirectToAction(nameof(All));
        }
    }
}
