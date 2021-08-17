using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Services.Recipes.Models;

namespace HealthyLifestyleTrackingApp.Services.Recipes
{
    public interface IRecipeService
    {
        RecipeQueryServiceModel All(
          string searchTerm,
          int currentPage,
          int RecipesPerPage);

        RecipeDetailsServiceModel Details(int recipeId);

        int Create(
            string name,
            string imageUrl,
            int servingsCount,
            int caloriesPerServing,
            int readyIn,
            string instructions,
            int lifeCoachId);

        bool Edit(
            int recipeId,
            string name,
            string imageUrl,
            int servingsCount,
            int caloriesPerServing,
            int readyIn,
            string instructions);

        void Delete(int id);

        bool RecipeIsByLifeCoach(int recipeId, int lifeCoachId);

        IEnumerable<RecipeServiceModel> ByUser(string userId);
    }
}
