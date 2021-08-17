using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Services.Articles.Models;

namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public interface IArticleService
    {
        ArticleQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int ArticlesPerPage);

        ArticleDetailsServiceModel Details(int articleId);

        int Create(
            string title,
            string content,
            string imageUrl,
            int lifeCoachId);

        bool Edit(
            int articleId,
            string title,
            string content,
            string imageUrl);

        void Delete(int id);

        bool ArticleIsByLifeCoach(int articleId, int lifeCoachId);

        IEnumerable<ArticleServiceModel> ByUser(string userId);
    }
}
 