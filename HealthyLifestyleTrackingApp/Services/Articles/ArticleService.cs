using System;
using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Services.Articles.Models;

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
                .OrderByDescending(a => a.CreatedOn)
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

        public ArticleDetailsServiceModel Details(int articleId)
            => this.data
                .Articles
                .Where(a => a.Id == articleId)
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
                CreatedOn = DateTime.Now,
                Content = content,
                ImageUrl = imageUrl,
                LifeCoachId = lifeCoachId
            };

            this.data.Articles.Add(articleData);
            this.data.SaveChanges();

            return articleData.Id;
        }

        public bool Edit(int articleId, string title, string content, string imageUrl)
        {
            var articleData = this.data.Articles.Find(articleId);

            if (articleData == null)
            {
                return false;
            }

            articleData.Title = title;
            articleData.Content = content;
            articleData.ImageUrl = imageUrl;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var article = this.data.Articles.Where(c => c.Id == id).FirstOrDefault();

            if (article == null)
            {
                return false;
            }

            this.data.Remove(article);
            this.data.SaveChanges();

            return true;
        }


        public bool ArticleExists(int articleId)
            => this.data
                .Articles
                .Any(a => a.Id == articleId);


        public bool ArticleIsByLifeCoach(int articleId, int lifeCoachId)
            => this.data
                .Articles
                .Any(a => a.Id == articleId && a.LifeCoachId == lifeCoachId);


        public IEnumerable<ArticleServiceModel> ByUser(string userId)
            => this.GetArticles(this.data
                .Articles
                .Where(a => a.LifeCoach.UserId == userId));


        private IEnumerable<ArticleServiceModel> GetArticles(IQueryable<Article> ariclesQuery)
            => ariclesQuery
                .Select(a => new ArticleServiceModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Author = a.LifeCoach.FirstName + " " + a.LifeCoach.LastName,
                    CreatedOn = a.CreatedOn,
                    ImageUrl = a.ImageUrl
                })
                    .ToList();
    }
}
