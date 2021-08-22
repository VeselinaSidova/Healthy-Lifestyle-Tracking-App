using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Recipes;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using HealthyLifestyleTrackingApp.Services.Recipes;
using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IRecipeService recipes;
        private readonly ILifeCoachService lifeCoaches;

        public RecipesController(IRecipeService recipes, ILifeCoachService lifeCoaches)
        {
            this.recipes = recipes;
            this.lifeCoaches = lifeCoaches;
        }

        public IActionResult All([FromQuery] AllRecipesQueryModel query)
        {
            var queryResult = this.recipes.All(
                 query.SearchTerm,
                 query.CurrentPage,
                 AllRecipesQueryModel.RecipesPerPage);

            query.Recipes = queryResult.Recipes;
            query.TotalRecipes = queryResult.TotalRecipes;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myArticles = this.recipes.ByUser(this.User.GetId());

            return View(myArticles);
        }

        [Authorize]
        public IActionResult Read(int id, string information)
        {
            var recipe = this.recipes.Details(id);

            if (!information.Contains(recipe.Name))
            {
                return BadRequest();
            }

            return View(recipe);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!this.lifeCoaches.IsLifeCoach(this.User.GetId()))
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(RecipeFormModel recipe)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0)
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            this.recipes.Create(
                recipe.Name,
                recipe.ImageUrl,
                recipe.ServingsCount,
                recipe.CaloriesPerServing,
                recipe.ReadyIn,
                recipe.Instructions,
                lifeCoachId);

            TempData[GlobalMessageKey] = "Recipe was successfully created.";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.lifeCoaches.IsLifeCoach(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            var recipe = this.recipes.Details(id);

            if (recipe.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new RecipeFormModel
            {
                Name = recipe.Name,
                ServingsCount = recipe.ServingsCount,
                ImageUrl = recipe.ImageUrl,
                CaloriesPerServing = recipe.CaloriesPerServing,
                ReadyIn = recipe.ReadyIn,
                Instructions = recipe.Instructions
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, RecipeFormModel recipe)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!ModelState.IsValid)
            {
                return View(recipe);
            }


            if (!this.recipes.RecipeIsByLifeCoach(id, lifeCoachId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            var recipeIsEdited = recipes.Edit(
               id,
               recipe.Name,
               recipe.ImageUrl,
               recipe.ServingsCount,
               recipe.CaloriesPerServing,
               recipe.ReadyIn,
               recipe.Instructions);

            TempData[GlobalMessageKey] = "Recipe was successfully edited.";

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId()); 

            if (!this.recipes.RecipeIsByLifeCoach(id, lifeCoachId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.recipes.Delete(id);

            return RedirectToAction(nameof(All));
        }
    }
}
