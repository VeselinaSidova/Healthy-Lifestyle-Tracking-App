using System;

namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public class ArticleServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Author { get; init; }

        public DateTime CreatedOn { get; init; }

        public string ImageUrl { get; init; }
    }
}
