using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Services.Articles;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (lifeCoachId == 0)
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            var myArticles = this.articles.ByUser(this.User.GetId());

            return View(myArticles);
        }

        [Authorize]
        public IActionResult Read(int id, string information)
        {
            var article = this.articles.Details(id);

            if (article == null)
            {
                return NotFound("Article not found!");
            }

            if (article != null && !information.Contains(article.Title))
            {
                return BadRequest("Article with such data does not exist!");
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
            var article = this.articles.Details(id);

            if (article == null)
            {
                return NotFound("Article not found!");
            }

            if (article.UserId != userId  && !User.IsAdmin())
            {
                return Unauthorized("You cannot edit this article!");
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
            if (!this.articles.ArticleExists(id))
            {
                return NotFound("Article not found!");
            }

            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());

            if (!this.articles.ArticleIsByLifeCoach(id, lifeCoachId) && !User.IsAdmin())
            {
                return Unauthorized("You cannot edit this article!");
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var articleIsEdited = articles.Edit(
               id,
               article.Title,
               article.Content,
               article.ImageUrl);

            if (articleIsEdited == false)
            {
                return BadRequest("Article not edited!");
            }


            TempData[GlobalMessageKey] = "Article was successfully edited.";

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction(nameof(All));
            }

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!this.articles.ArticleExists(id))
            {
                return NotFound("Article not found!");
            }

            var lifeCoachId = this.lifeCoaches.GetIdByUser(this.User.GetId());         

            if (!this.articles.ArticleIsByLifeCoach(id, lifeCoachId) && !User.IsAdmin())
            {
                return Unauthorized("You cannot delete this article!");
            }

            var isDeleted = this.articles.Delete(id);

            if (isDeleted == false)
            {
                return BadRequest("Article not deleted!");
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction(nameof(All));
            }

            return RedirectToAction(nameof(Mine));
        }
    }
}
