using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public class ArticleQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ArticlesPerPage { get; init; }

        public int TotalArticles { get; init; }

        public IEnumerable<ArticleServiceModel> Articles { get; init; }
    }
}
