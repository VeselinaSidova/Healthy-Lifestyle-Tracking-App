using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public interface IArticleService
    {
        ArticleQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int ArticlesPerPage);

        IEnumerable<ArticleServiceModel> ByUser(string userId);
    }
}
 