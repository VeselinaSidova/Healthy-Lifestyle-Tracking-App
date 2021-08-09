namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public class ArticleDetailsServiceModel : ArticleServiceModel
    {
        public string Content { get; init; }

        public int LifeCoachId { get; init; }

        public string UserId { get; init; }
    }
}
