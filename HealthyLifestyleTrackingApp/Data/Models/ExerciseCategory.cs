using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static HealthyLifestyleTrackingApp.Data.DataConstants.Category;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class ExerciseCategory
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLenghth)]
        public string Name { get; set; }

        public IEnumerable<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
