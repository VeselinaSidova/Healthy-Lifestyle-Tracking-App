using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Exercise;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public ExercisesController(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public IActionResult Create() => View(new CreateExerciseFormModel
        {
            ExerciseCategories = this.GetExerciseCategories(),
        });

        public IActionResult All()
        {
            var exercises = this.data
                .Exercises
                .OrderBy(e => e.ExerciseCategory.Name)
                .Select(e => new 
                {
                    Id = e.Id,
                    Name = e.Name,
                    CaloriesPerHous = e.CaloriesPerHour,
                    ImageUrl = e.ImageUrl,
                    ExerciseCategory = e.ExerciseCategory.Name
                })
                .ToList();

            return View(exercises);
        }

        [HttpPost]
        public IActionResult Create(CreateExerciseFormModel exercise)
        {
            if (!this.data.ExerciseCategories.Any(c => c.Id == exercise.ExerciseCategoryId))
            {
                this.ModelState.AddModelError(nameof(exercise.ExerciseCategoryId), "Exercise category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                exercise.ExerciseCategories = this.GetExerciseCategories();
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

        private IEnumerable<ExerciseCategoryViewModel> GetExerciseCategories()
             => this.data
            .ExerciseCategories
            .Select(c => new ExerciseCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();
        
    }
}
