using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.Exercise;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Exercise
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int CaloriesPerHour { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ExerciseCategoryId { get; set; }

        public ExerciseCategory ExerciseCategory { get; set; }

        public IEnumerable<TrackedExercise> TrackedExercises { get; set; } = new List<TrackedExercise>();
    }
}
