using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Exercises;
using HealthyLifestyleTrackingApp.Services.Exercises;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IExerciseService exercises;
        private readonly HealthyLifestyleTrackerDbContext data;
        private readonly ILifeCoachService lifeCoaches;

        public ExercisesController(IExerciseService exercises, ILifeCoachService lifeCoaches, HealthyLifestyleTrackerDbContext data)
        {
            this.exercises = exercises;
            this.lifeCoaches = lifeCoaches;
            this.data = data;
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

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateExerciseFormModel exercise)
        {
            var lifeCoachId = this.data
                .LifeCoaches
                .Where(c => c.UserId == this.User.GetId())
                .Select(c => c.Id)
                .FirstOrDefault();

            if (lifeCoachId == 0)
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!this.data.ExerciseCategories.Any(c => c.Id == exercise.ExerciseCategoryId))
            {
                this.ModelState.AddModelError(nameof(exercise.ExerciseCategoryId), "Exercise category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                exercise.ExerciseCategories = this.exercises.GetExerciseCategories();
                return View(exercise);
            }

            var exerciseData = new Exercise
            {
                Name = exercise.Name,
                CaloriesPerHour = exercise.CaloriesPerHour,
                ImageUrl = exercise.ImageUrl,
                ExerciseCategoryId = exercise.ExerciseCategoryId
            };

            this.data.Exercises.Add(exerciseData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }
}
