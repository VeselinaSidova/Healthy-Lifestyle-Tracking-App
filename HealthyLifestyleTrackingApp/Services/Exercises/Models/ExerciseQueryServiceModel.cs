using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Exercises.Models
{
    public class ExerciseQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ExercisesPerPage { get; init; }

        public int TotalExercises { get; set; }

        public IEnumerable<ExerciseServiceModel> Exercises { get; init; }
    }
}
