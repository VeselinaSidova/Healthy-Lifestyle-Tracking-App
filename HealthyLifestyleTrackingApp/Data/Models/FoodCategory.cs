using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class FoodCategory
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(CategoryNameMaxLenghth)]
        public string Name { get; set; }

        public IEnumerable<Food> Foods { get; set; } = new List<Food>();
    }
}
