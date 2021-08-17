using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Services.Foods.Models
{
    public class FoodTagServiceModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }
    }
}
