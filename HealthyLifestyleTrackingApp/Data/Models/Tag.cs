using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static HealthyLifestyleTrackingApp.Data.DataConstants.FoodTag;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<FoodTag> FoodTags { get; set; } = new List<FoodTag>();
    }
}
