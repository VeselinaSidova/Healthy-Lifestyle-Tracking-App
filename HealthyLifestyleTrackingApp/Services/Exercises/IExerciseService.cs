using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data.Enums;

namespace HealthyLifestyleTrackingApp.Services.Exercises
{
    public interface IExerciseService
    {
        ExerciseQueryServiceModel All(
            string category,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int exercisesPerPage);

        int Create(string name, 
            int caloriesPerHour,
            string imageUrl,
            int exerciseCategoryId);

        int Track(
            int exerciseId,
            string userId,
            int duration);

        IEnumerable<ExerciseCategoryServiceModel> GetExerciseCategories();

        string GetExerciseName(int exerciseId);

        bool ExerciseCategoryExists(int categoryId);

    }
} 
