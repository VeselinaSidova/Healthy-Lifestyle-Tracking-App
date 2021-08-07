using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Services.Exercises;

namespace HealthyLifestyleTrackingApp.Models.Exercises
{
    public class AllExercisesQueryModel
    {
        public const int ExercisesPerPage = 8;

        public string Category { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public Sorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalExercises { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ExerciseServiceModel> Exercises { get; set; }
    }
}
