using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Services.Exercises.Models;

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

        ExerciseDetailsServiceModel Details(int exerciseId);

        int Create(string name, 
            int caloriesPerHour,
            string imageUrl,
            int exerciseCategoryId);

        bool Edit(
            int exerciseId,
            string name,
            int caloriesPerHour,
            string imageUrl,
            int exerciseCategoryId);

        int Track(
            int exerciseId,
            string userId,
            int duration);

        void Delete(int id);

        IEnumerable<ExerciseCategoryServiceModel> GetExerciseCategories();

        string GetExerciseName(int exerciseId);

        bool ExerciseCategoryExists(int categoryId);

    }
} 
