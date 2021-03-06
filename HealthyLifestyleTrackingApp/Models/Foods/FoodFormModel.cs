using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Services.Foods.Models;

using static HealthyLifestyleTrackingApp.Data.DataConstants.Food;

namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class FoodFormModel : IValidatableObject
    {
        private const int NutritionalValueSum = 100;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Food name should be between 2 and 30 characters long.")]
        public string Name { get; init; }


        [StringLength(BrandMaxLength, MinimumLength = BrandMinLength, ErrorMessage = "Food brand name should be between 1 and 20 characters long")]
        public string Brand { get; init; }

        [Display(Name = "Standard Serving Amount")]
        [Required]
        [Range(StandardServingAmountMinValue, StandardServingAmountMaxValue, ErrorMessage = "Value should be a number between 0.1 to 5000 grams.")]
        public double? StandardServingAmount { get; set; }

        [Required]
        [Display(Name = "Standard Serving Type")]
        public StandardServingType StandardServingType { get; set; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        [Range(CaloriesMinValue, CaloriesMaxValue, ErrorMessage = "Value should be a whole number between 0 and 10000.")]
        public int? Calories { get; init; }

        [Required]
        [Range(NutritionMinValue, NutritionMaxValue, ErrorMessage = "Value should be between 0 and 100 grams per 100 grams.")]
        public double? Protein { get; init; }

        [Required]
        [Range(NutritionMinValue, NutritionMaxValue, ErrorMessage = "Value should be between 0 and 100 grams per 100 grams.")]
        public double? Carbohydrates { get; init; }

        [Required]
        [Range(NutritionMinValue, NutritionMaxValue, ErrorMessage = "Value should be between 0 and 100 grams per 100 grams.")]
        public double? Fat { get; init; }

        [Required]
        [Display(Name = "Category")]
        public int FoodCategoryId { get; init; }

        public IEnumerable<FoodCategoryServiceModel> FoodCategories { get; set; }

        public IEnumerable<FoodTagServiceModel> Tags { get; set; }

        public ICollection<int> FoodTags { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Protein + this.Carbohydrates + this.Fat > NutritionalValueSum)
            {
                yield return new ValidationResult("The sum of the nutritional values for protein, carbohydrates and fats should be less or equal to 100 grams!");
            }
        }
    }
}
