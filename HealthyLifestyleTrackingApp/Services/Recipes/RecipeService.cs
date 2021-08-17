using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Services.Recipes.Models;

namespace HealthyLifestyleTrackingApp.Services.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public RecipeService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public RecipeQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int recipesPerPage)
        {
            var recipeQuery = this.data.Recipes.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                recipeQuery = recipeQuery.Where(a =>
                    a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalRecipes = recipeQuery.Count();

            var recipes = GetRecipes(recipeQuery
                .OrderBy(r => r.Name)
                .Skip((currentPage - 1) * recipesPerPage)
                .Take(recipesPerPage));


            return new RecipeQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalRecipes = totalRecipes,
                RecipesPerPage = recipesPerPage,
                Recipes = recipes
            };
        }

        public RecipeDetailsServiceModel Details(int recipeId)
            => this.data
                 .Recipes
                 .Where(r => r.Id == recipeId)
                 .Select(r => new RecipeDetailsServiceModel
                 {
                     Id = r.Id,
                     Name = r.Name,
                     Author = r.LifeCoach.FirstName + " " + r.LifeCoach.LastName,
                     ImageUrl = r.ImageUrl,
                     ServingsCount = r.ServingsCount,
                     CaloriesPerServing = r.CaloriesPerServing,
                     ReadyIn = r.ReadyIn,
                     Instructions = r.Instructions,
                     LifeCoachId = r.LifeCoachId,
                     UserId = r.LifeCoach.UserId
                 })
                 .FirstOrDefault();


        public int Create(string name, string imageUrl, int servingsCount, int caloriesPerServing, int readyIn, string instructions, int lifeCoachId)
        {
            var recipeData = new Recipe
            {
                Name = name,
                ImageUrl = imageUrl,
                ServingsCount = servingsCount,
                CaloriesPerServing = caloriesPerServing,
                Instructions = instructions,
                ReadyIn = readyIn,
                LifeCoachId = lifeCoachId
            };

            this.data.Recipes.Add(recipeData);
            this.data.SaveChanges();

            return recipeData.Id;

        }

        public bool Edit(int recipeId, string name, string imageUrl, int servingsCount, int caloriesPerServing, int readyIn, string instructions)
        {
            var recipeData = this.data.Recipes.Find(recipeId);

            if (recipeData == null)
            {
                return false;
            }

            recipeData.Name = name;
            recipeData.ImageUrl = imageUrl;
            recipeData.ServingsCount = servingsCount;
            recipeData.CaloriesPerServing = caloriesPerServing;
            recipeData.ReadyIn = readyIn;
            recipeData.Instructions = instructions;

            this.data.SaveChanges();

            return true;
        }


        public void Delete(int id)
        {
            var recipe = this.data.Recipes.Where(c => c.Id == id).FirstOrDefault();

            this.data.Remove(recipe);

            this.data.SaveChanges();
        }


        public bool RecipeIsByLifeCoach(int recipeId, int lifeCoachId)
            => this.data
                .Recipes
                .Any(a => a.Id == recipeId && a.LifeCoachId == lifeCoachId);

        public IEnumerable<RecipeServiceModel> ByUser(string userId)
            => this.GetRecipes(this.data
                .Recipes
                .Where(a => a.LifeCoach.UserId == userId));

        private IEnumerable<RecipeServiceModel> GetRecipes(IQueryable<Recipe> recipesQuery)
           => recipesQuery
               .Select(r => new RecipeServiceModel
               {
                   Id = r.Id,
                   Name = r.Name,
                   ImageUrl = r.ImageUrl,
                   Author = r.LifeCoach.FirstName + " " + r.LifeCoach.LastName,
                   ServingsCount = r.ServingsCount,
                   CaloriesPerServing = r.CaloriesPerServing,
                   ReadyIn = r.ReadyIn,
               })
                   .ToList();
    }
}
