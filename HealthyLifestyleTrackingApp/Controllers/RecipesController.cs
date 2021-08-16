using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using HealthyLifestyleTrackingApp.Services.Recipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        //public IActionResult All([FromQuery] AllArticlesQueryModel query)
        //{
        //    var queryResult = this.articles.All(
        //         query.SearchTerm,
        //         query.CurrentPage,
        //         AllArticlesQueryModel.ArticlesPerPage);

        //    query.Articles = queryResult.Articles;
        //    query.TotalArticles = queryResult.TotalArticles;

        //    return View(query);
        //}

        //[Authorize]
        //public IActionResult Mine()
        //{
        //    var myArticles = this.recipes.ByUser(this.User.GetId());

        //    return View(myArticles);
        //}

        //[Authorize]
        //public IActionResult Create()
        //{
        //    if (!this.lifeCoaches.IsLifeCoach(this.User.GetId()))
        //    {
        //        return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
        //    }

        //    return View();
        //}

        //[HttpPost]
        //[Authorize]
        //public IActionResult Create(ArticleFormModel recipe)
        //{
        //    var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

        //    if (lifeCoachId == 0)
        //    {
        //        return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(recipe);
        //    }

        //    this.recipes.Create();

        //    return RedirectToAction(nameof(All));
        //}
    }
}
