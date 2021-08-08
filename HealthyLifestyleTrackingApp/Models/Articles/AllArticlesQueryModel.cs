using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Services.Articles;

namespace HealthyLifestyleTrackingApp.Models.Articles
{
    public class AllArticlesQueryModel
    {
        public const int ArticlesPerPage = 4;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalArticles { get; set; }

        public IEnumerable<ArticleServiceModel> Articles { get; set; }
    }
}
