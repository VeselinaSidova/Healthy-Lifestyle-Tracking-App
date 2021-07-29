using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Data.Enums;
using static HealthyLifestyleTrackingApp.Data.DataConstants;


namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Food
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(FoodNameMaxLength)]
        public string Name { get; set; }

        [MaxLength(FoodBrandNameMaxLength)]
        public string Brand { get; set; }

        public double Amount { get; set; }

        public ServingType ServingType { get; set; }

         public int Calories { get; set; }

        public double Protein { get; set; }

        public double Carbohydrates { get; set; }

        public double Fat { get; set; }

        public int FoodCategoryId { get; set; }

        public FoodCategory FoodCategory { get; init; }

        public ICollection<FoodTag> FoodTags { get; set; }
    }
}
