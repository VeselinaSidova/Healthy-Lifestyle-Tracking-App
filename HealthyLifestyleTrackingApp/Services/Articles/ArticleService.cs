using HealthyLifestyleTrackingApp.Data;
using System.Linq;

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

            var articles = articleQuery
                .Skip((currentPage - 1) * articlesPerPage)
                .Take(articlesPerPage)
                .Select(a => new ArticleServiceModel
                {
                   Title = a.Title,
                   Content = a.Content, 
                   Author = a.LifeCoach.FirstName + " " + a.LifeCoach.LastName,
                   CreatedOn = a.CreatedOn.Date,
                   ImageUrl = a.ImageUrl
                })
                .ToList();

            return new ArticleQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalArticles = totalArticles,
                ArticlesPerPage = articlesPerPage,
                Articles = articles
            };
        }
    }
}
