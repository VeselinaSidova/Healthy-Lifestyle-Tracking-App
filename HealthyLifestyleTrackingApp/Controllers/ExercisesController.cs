using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Exercises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Services.Exercises;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IExerciseService exercises;
        private readonly HealthyLifestyleTrackerDbContext data;

        public ExercisesController(IExerciseService exercises, HealthyLifestyleTrackerDbContext data)
        {
            this.exercises = exercises;
            this.data = data;
        }


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
            var queryResult = this.exercises.All(
                 query.Category,
                 query.SearchTerm,
                 query.Sorting,
                 query.CurrentPage,
                 AllExercisesQueryModel.ExercisesPerPage);

            var exerciseCategories = this.exercises.AllExerciseCategories();

            query.Categories = exerciseCategories;
            query.Exercises = queryResult.Exercises;
            query.TotalExercises = queryResult.TotalExercises;

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
