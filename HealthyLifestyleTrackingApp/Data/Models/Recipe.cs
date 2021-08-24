using System.ComponentModel.DataAnnotations;

using static HealthyLifestyleTrackingApp.Data.DataConstants.Recipe;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int ServingsCount { get; set; }

        public int CaloriesPerServing { get; set; }

        public int ReadyIn { get; set; }

        [Required]
        public string Instructions { get; set; }

        public int LifeCoachId { get; init; }

        public LifeCoach LifeCoach { get; set; }

    }
}
