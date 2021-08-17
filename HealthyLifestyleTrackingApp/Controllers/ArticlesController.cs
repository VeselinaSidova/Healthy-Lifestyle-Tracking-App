using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Services.Articles;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService articles;
        private readonly ILifeCoachService lifeCoaches;

        public ArticlesController(IArticleService articles, ILifeCoachService lifeCoaches)
        {
            this.articles = articles;
            this.lifeCoaches = lifeCoaches;
        }

        [Authorize]
        public IActionResult All([FromQuery] AllArticlesQueryModel query)
        {
            var queryResult = this.articles.All(
                 query.SearchTerm,
                 query.CurrentPage,
                 AllArticlesQueryModel.ArticlesPerPage);

            query.Articles = queryResult.Articles;
            query.TotalArticles = queryResult.TotalArticles;

            return View(query);
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myArticles = this.articles.ByUser(this.User.GetId());

            return View(myArticles);
        }

        [Authorize]
        public IActionResult Read(int id, string information)
        {
            var article = this.articles.Details(id);

            if (!information.Contains(article.Title))
            {
                return BadRequest();
            }

            return View(article);
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
        public IActionResult Create(ArticleFormModel article)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0)
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            this.articles.Create(
                article.Title,
                article.Content,
                article.ImageUrl,
                lifeCoachId);

            TempData[GlobalMessageKey] = "Article was successfully created.";

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();

            if (!this.lifeCoaches.IsLifeCoach(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            var article = this.articles.Details(id);

            if (article.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            return View(new ArticleFormModel
            {
                Title = article.Title,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, ArticleFormModel article)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }


            if (!this.articles.ArticleIsByLifeCoach(id, lifeCoachId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            var articleIsEdited = articles.Edit(
               id,
               article.Title,
               article.Content,
               article.ImageUrl);


            TempData[GlobalMessageKey] = "Article was successfully edited.";

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (!this.articles.ArticleIsByLifeCoach(id, lifeCoachId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.articles.Delete(id);

            TempData[GlobalMessageKey] = "Article was deleted.";

            return RedirectToAction(nameof(All));
        }
    }
}
