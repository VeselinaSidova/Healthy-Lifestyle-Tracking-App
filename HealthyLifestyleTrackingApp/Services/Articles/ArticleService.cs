using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using System;

namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public class ArticleService : IArticleService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public ArticleService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public ArticleQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int articlesPerPage)
        {
            var articleQuery = this.data.Articles.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                articleQuery = articleQuery.Where(a =>
                    a.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalArticles = articleQuery.Count();

            var articles = GetArticles(articleQuery
                .Skip((currentPage - 1) * articlesPerPage)
                .Take(articlesPerPage));
                

            return new ArticleQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalArticles = totalArticles,
                ArticlesPerPage = articlesPerPage,
                Articles = articles
            };
        }

        public ArticleServiceModel Details(int id)
            => this.data
                .Articles
                .Where(a => a.Id == id)
                .Select(a => new ArticleDetailsServiceModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Author = a.LifeCoach.FirstName + " " + a.LifeCoach.LastName,
                    CreatedOn = a.CreatedOn.Date,
                    Content = a.Content,
                    ImageUrl = a.ImageUrl,
                    LifeCoachId = a.LifeCoachId,
                    UserId = a.LifeCoach.UserId
                })
                .FirstOrDefault();

        public int Create(string title, string content, string imageUrl, int lifeCoachId)
        {
            var articleData = new Article
            {
                Title = title,
                CreatedOn = DateTime.UtcNow,
                Content = content,
                ImageUrl = imageUrl,
                LifeCoachId = lifeCoachId
            };

            this.data.Articles.Add(articleData);
            this.data.SaveChanges();

            return articleData.Id;
        }

        public IEnumerable<ArticleServiceModel> ByUser(string userId)
            => this.GetArticles(this.data
                .Articles
                .Where(a => a.LifeCoach.UserId == userId));


        private IEnumerable<ArticleServiceModel> GetArticles(IQueryable<Article> ariclesQuery)
            => ariclesQuery
                .Select(a => new ArticleServiceModel
                {
                    Title = a.Title,
                    Author = a.LifeCoach.FirstName + " " + a.LifeCoach.LastName,
                    CreatedOn = a.CreatedOn,
                    ImageUrl = a.ImageUrl
                })
                    .ToList();

       
    }
}
