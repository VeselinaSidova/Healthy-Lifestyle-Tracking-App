using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants;

namespace HealthyLifestyleTrackingApp.Models.Exercise
{
    public class CreateExerciseFormModel
    {
        [Required]
        [StringLength(ExerciseNameMaxLength, MinimumLength = ExerciseNameMinLength, ErrorMessage = "Exercise name should be between 2 and 40 characters long.")]
        public string Name { get; init; }

        [Required]
        [Range(ExerciseCaloriesPerHourMinValue, ExerciseCaloriesPerHourMaxValue, ErrorMessage = "Value should be a whole number between 0 and 10000.")]
        public int CaloriesPerHour { get; init; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        [Display(Name = "Category")]
        public int ExerciseCategoryId { get; init; }

        public IEnumerable<ExerciseCategoryViewModel> ExerciseCategories { get; set; }
    }
}
