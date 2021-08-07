using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Exercises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Infrastructure;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public ExercisesController(HealthyLifestyleTrackerDbContext data)
            => this.data = data;


        [Authorize]
        public IActionResult Create()
        {
            if (!this.UserIsLifeCoach())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            return View(new CreateExerciseFormModel
            {
                ExerciseCategories = this.GetExerciseCategories(),
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

            var totalExercises = exerciseQuery.Count();

            var exercises = exerciseQuery
                .Skip((query.CurrentPage - 1) * AllExercisesQueryModel.ExercisesPerPage)
                .Take(AllExercisesQueryModel.ExercisesPerPage)
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
            query.TotalExercises = totalExercises;

            return View(query);
        }

        private bool UserIsLifeCoach() 
            => this.data
                .LifeCoaches
                .Any(c => c.UserId == this.User.GetId());

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
