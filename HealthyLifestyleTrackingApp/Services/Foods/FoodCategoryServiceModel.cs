using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Service.Foods
{
    public class FoodCategoryServiceModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
