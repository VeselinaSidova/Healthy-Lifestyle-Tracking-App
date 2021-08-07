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

        IEnumerable<string> AllExerciseCategories();
    }
}
