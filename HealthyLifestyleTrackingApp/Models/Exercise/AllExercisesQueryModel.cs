using HealthyLifestyleTrackingApp.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Models.Exercise
{
    public class AllExercisesQueryModel
    {
        public string Category { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public Sorting Sorting { get; init; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ExerciseListingViewModel> Exercises { get; set; }
    }
}
