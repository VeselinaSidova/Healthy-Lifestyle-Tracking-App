using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.Recipe;

namespace HealthyLifestyleTrackingApp.Models.Recipes
{
    public class RecipeFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Recipe name should be between 2 and 50 characters long.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        [Range(ServingsCountMinValue, ServingsCountMaxValue, ErrorMessage = "Value should be a whole number between 1 and 20.")]
        [Display(Name = "Number Of Servings")]
        public int ServingsCount { get; init; }

        [Required]
        [Range(CaloriesPerServingMinValue, CaloriesPerServingMaxValue, ErrorMessage = "Value should be a whole number between 0 and 10000.")]
        [Display(Name = "Calories Per Serving")]
        public int CaloriesPerServing { get; init; }

        [Required]
        [Range(ReadyInMinValue, ReadyInMaxValue, ErrorMessage = "Value should be a whole number between 1 and 1000 mins.")]
        [Display(Name = "Ready In")]
        public int ReadyIn { get; set; }

        [Required]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string Instructions { get; init; }
    }
}
