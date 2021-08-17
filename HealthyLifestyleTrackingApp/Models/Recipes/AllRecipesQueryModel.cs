using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Services.Recipes.Models;

namespace HealthyLifestyleTrackingApp.Models.Recipes
{
    public class AllRecipesQueryModel
    {
        public const int RecipesPerPage = 4;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalRecipes { get; set; }

        public IEnumerable<RecipeServiceModel> Recipes { get; set; }
    }
}
