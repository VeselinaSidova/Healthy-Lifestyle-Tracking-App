using System;
using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public interface IArticleService
    {
        ArticleQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int ArticlesPerPage);

        ArticleServiceModel Details(int id);

        int Create(
            string title,
            string content,
            string imageUrl,
            int lifeCoachId);

        IEnumerable<ArticleServiceModel> ByUser(string userId);
    }
}
 