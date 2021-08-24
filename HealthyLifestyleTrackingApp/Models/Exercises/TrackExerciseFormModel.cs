using System.ComponentModel.DataAnnotations;

using static HealthyLifestyleTrackingApp.Data.DataConstants.TrackedExercise;

namespace HealthyLifestyleTrackingApp.Models.Exercises
{
    public class TrackExerciseFormModel
    {
        [Required]
        [Range(DurationMinValue, DurationMaxValue, ErrorMessage = "Value should be between 1 and 1000 minutes.")]
        public int Duration { get; init; }
    }
}
