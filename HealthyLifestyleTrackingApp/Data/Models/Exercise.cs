using System;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Exercise
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(ExerciseNameMaxLength)]
        public string Name { get; set; }

        public int CaloriesPerHour { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ExerciseCategoryId { get; set; }

        public ExerciseCategory ExerciseCategory { get; set; }
    }
}
