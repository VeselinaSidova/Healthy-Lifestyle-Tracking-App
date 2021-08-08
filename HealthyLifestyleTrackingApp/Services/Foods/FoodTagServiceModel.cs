using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Service.Foods
{
    public class FoodTagServiceModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
