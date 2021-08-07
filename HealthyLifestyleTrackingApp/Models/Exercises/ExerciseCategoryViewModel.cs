using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Models.Exercises
{
    public class ExerciseCategoryViewModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
