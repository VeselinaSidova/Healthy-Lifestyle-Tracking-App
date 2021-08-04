using HealthyLifestyleTrackingApp.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants;

namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class CreateFoodFormModel
    {
        [Required]
        [StringLength(FoodNameMaxLength, MinimumLength = FoodNameMinLength, ErrorMessage = "Food name should be between 2 and 30 characters long.")]
        public string Name { get; init; }


        [StringLength(FoodBrandMaxLength, MinimumLength = FoodBrandMinLength, ErrorMessage = "Food brand name should be between 1 and 20 characters long")]
        public string Brand { get; init; }

        [Display(Name = "Standard Serving Amount")]
        [Required]
        [Range(FoodStandardServingAmountMinValue, FoodStandardServingAmountMaxValue, ErrorMessage = "Value should be a number between 0.1 to 5000 grams.")]
        public double? StandardServingAmount { get; set; }

        [Required]
        [Display(Name = "Standard Serving Type")]
        public StandardServingType StandardServingType { get; set; }

        [Required]
        [Range(FoodCaloriesMinValue, FoodCaloriesMaxValue, ErrorMessage = "Value should be a whole number between 0 and 10000.")]
        public int? Calories { get; init; }

        [Required]
        [Range(FoodNutritionMinValue, FoodNutritionMaxValue, ErrorMessage = "Value should be between 0 and 100 grams per 100 grams.")]
        public double? Protein { get; init; }

        [Required]
        [Range(FoodNutritionMinValue, FoodNutritionMaxValue, ErrorMessage = "Value should be between 0 and 100 grams per 100 grams.")]
        public double? Carbohydrates { get; init; }

        [Required]
        [Range(FoodNutritionMinValue, FoodNutritionMaxValue, ErrorMessage = "Value should be between 0 and 100 grams per 100 grams.")]
        public double? Fat { get; init; }

        [Required]
        [Display(Name = "Category")]
        public int FoodCategoryId { get; init; }

        public IEnumerable<FoodCategoryViewModel> FoodCategories { get; set; }

        public IEnumerable<FoodTagViewModel> Tags { get; set; }

        public ICollection<int> FoodTags { get; set; }
    }
}
