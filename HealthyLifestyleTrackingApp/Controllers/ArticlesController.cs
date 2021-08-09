using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Services.Articles;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService articles;
        private readonly HealthyLifestyleTrackerDbContext data;
        private readonly ILifeCoachService lifeCoaches;

        public ArticlesController(IArticleService articles, ILifeCoachService lifeCoaches, HealthyLifestyleTrackerDbContext data)
        {
            this.articles = articles;
            this.lifeCoaches = lifeCoaches;
            this.data = data;
        }

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
        public IActionResult Create()
        {
            if (!this.lifeCoaches.IsLifeCoach(this.User.GetId()))
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            return View();
        }


        [Authorize]
        [HttpPost]
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

            return RedirectToAction(nameof(All));
        }

        //[Authorize]
        //public IActionResult Edit(int id)
        //{

        //}
    }
}
