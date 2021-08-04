using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class FoodTagViewModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
