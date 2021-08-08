using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Services.Articles;


namespace HealthyLifestyleTrackingApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService articles;
        private readonly HealthyLifestyleTrackerDbContext data;

        public ArticlesController(IArticleService articles, HealthyLifestyleTrackerDbContext data)
        {
            this.articles = articles;
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
        public IActionResult Create()
        {
            if (!this.UserIsLifeCoach())
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            return View();
        }


        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateArticleFormModel article)
        {
            var lifeCoachId = this.data
                .LifeCoaches
                .Where(c => c.UserId == this.User.GetId())
                .Select(c => c.Id)
                .FirstOrDefault();

            if (lifeCoachId == 0)
            {
                return RedirectToAction(nameof(LifeCoachesController.Become), "LifeCoaches");
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var articleData = new Article
            {
                Title = article.Title,
                CreatedOn = DateTime.UtcNow,
                Content = article.Content, 
                ImageUrl = article.ImageUrl,
                LifeCoachId = lifeCoachId
            };

            this.data.Articles.Add(articleData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private bool UserIsLifeCoach()
           => this.data
               .LifeCoaches
               .Any(c => c.UserId == this.User.GetId());
    }
}
