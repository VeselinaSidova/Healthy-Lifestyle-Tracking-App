using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
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

        public IActionResult All([FromQuery] AllExercisesQueryModel query)
        {
            var exerciseQuery = this.data.Exercises.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                exerciseQuery = exerciseQuery.Where(e => e.ExerciseCategory.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                exerciseQuery = exerciseQuery.Where(e =>
                    e.Name.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            exerciseQuery = query.Sorting switch
            {
                Sorting.Category => exerciseQuery.OrderBy(f => f.ExerciseCategory.Name),
                Sorting.DateCreated => exerciseQuery.OrderByDescending(f => f.Id),
                Sorting.Name or _ => exerciseQuery.OrderBy(f => f.Name)
            };

            var exerciseCategories = this.data.ExerciseCategories.Select(c => c.Name).OrderBy(c => c).Distinct().ToList();

            var exercises = exerciseQuery
                .Select(e => new ExerciseListingViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    CaloriesPerHour = e.CaloriesPerHour,
                    ImageUrl = e.ImageUrl,
                    ExerciseCategory = e.ExerciseCategory.Name
                })
                .ToList();

            query.Categories = exerciseCategories;
            query.Exercises = exercises;

            return View(query);
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
