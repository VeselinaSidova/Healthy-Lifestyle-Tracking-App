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
            if (!this.lifeCoaches.IsLifeCoach(this.User.GetId()) && !this.User.IsAdmin())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            return View(new ExerciseFormModel
            {
                ExerciseCategories = this.exercises.GetExerciseCategories(),
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ExerciseFormModel exercise)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0 && !this.User.IsAdmin())
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

            TempData[GlobalMessageKey] = "Exercise was successfully created.";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!User.IsAdmin())
            {
                return Unauthorized();
            }

            var exercise = this.exercises.Details(id);

            return View(new ExerciseFormModel
            {
                Name = exercise.Name,
                CaloriesPerHour = exercise.CaloriesPerHour,
                ImageUrl = exercise.ImageUrl,
                ExerciseCategoryId = exercise.ExerciseCategoryId,
                ExerciseCategories = this.exercises.GetExerciseCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, ExerciseFormModel exercise)
        {
            if (!User.IsAdmin())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(exercise);
            }

            var exerciseIsEdited = exercises.Edit(
               id,
               exercise.Name,
               exercise.CaloriesPerHour,
               exercise.ImageUrl,
               exercise.ExerciseCategoryId);

            TempData[GlobalMessageKey] = "Exercise was successfully edited.";

            return RedirectToAction(nameof(All));
        }


        [Authorize]
        public IActionResult Track()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Track(int id, string information, TrackExerciseFormModel exercise)
        {
            var userId = this.User.GetId();

            if (userId == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            var exerciseName = this.exercises.GetExerciseName(id);

            if (!information.Contains(exerciseName))
            {
                return BadRequest();
            }

            if (id == 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(exercise);
            }

            var trackedExerciseId = this.exercises.Track(
                id,
                userId,
                exercise.Duration);

            TempData[GlobalMessageKey] = "Exercise was added to tracker.";

            return RedirectToAction(nameof(All));
        }


        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.User.IsAdmin())
            {
                return BadRequest();
            }

            this.exercises.Delete(id);

            TempData[GlobalMessageKey] = "Exercise was deleted.";

            return RedirectToAction(nameof(All));
        }
    }
}
