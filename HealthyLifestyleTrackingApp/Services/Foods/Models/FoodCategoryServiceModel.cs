using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Services.Foods.Models
{
    public class FoodCategoryServiceModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
