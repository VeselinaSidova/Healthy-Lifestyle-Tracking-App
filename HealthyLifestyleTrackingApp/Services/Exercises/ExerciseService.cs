using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;

namespace HealthyLifestyleTrackingApp.Services.Exercises
{
    public class ExerciseService : IExerciseService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public ExerciseService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public ExerciseQueryServiceModel All(
            string category,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int exercisesPerPage)
        {
            var exerciseQuery = this.data.Exercises.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                exerciseQuery = exerciseQuery.Where(e => e.ExerciseCategory.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                exerciseQuery = exerciseQuery.Where(e =>
                    e.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            exerciseQuery = sorting switch
            {
                Sorting.Category => exerciseQuery.OrderBy(f => f.ExerciseCategory.Name),
                Sorting.DateCreated => exerciseQuery.OrderByDescending(f => f.Id),
                Sorting.Name or _ => exerciseQuery.OrderBy(f => f.Name)
            };

            var totalExercises = exerciseQuery.Count();

            var exercises = exerciseQuery
                .Skip((currentPage - 1) * exercisesPerPage)
                .Take(exercisesPerPage)
                .Select(e => new ExerciseServiceModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    CaloriesPerHour = e.CaloriesPerHour,
                    ImageUrl = e.ImageUrl,
                    ExerciseCategory = e.ExerciseCategory.Name
                })
                .ToList();

            return new ExerciseQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalExercises = totalExercises,
                ExercisesPerPage = exercisesPerPage,
                Exercises = exercises
            };
        }

        public IEnumerable<ExerciseCategoryServiceModel> GetExerciseCategories()
         => this.data
            .ExerciseCategories
            .Select(c => new ExerciseCategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();
    }
}
