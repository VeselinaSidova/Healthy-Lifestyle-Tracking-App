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

        [MaxLength(FoodBrandMaxLength)]
        public string Brand { get; set; }

        public double StandardServingAmount { get; set; }

        public StandardServingType StandardServingType { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int Calories { get; set; }

        public double Protein { get; set; }

        public double Carbohydrates { get; set; }

        public double Fat { get; set; }

        public int FoodCategoryId { get; set; }

        public FoodCategory FoodCategory { get; init; }

        public ICollection<FoodTag> FoodTags { get; set; } = new List<FoodTag>();
    }
}
