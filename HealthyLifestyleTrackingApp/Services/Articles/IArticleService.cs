namespace HealthyLifestyleTrackingApp.Services.Articles
{
    public interface IArticleService
    {
        ArticleQueryServiceModel All(
            string searchTerm,
            int currentPage,
            int ArticlesPerPage);
    }
}
