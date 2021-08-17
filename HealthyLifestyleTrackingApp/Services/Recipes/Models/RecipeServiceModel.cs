namespace HealthyLifestyleTrackingApp.Services.Recipes.Models
{
    public class RecipeServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Author { get; init; }

        public int ServingsCount { get; init; }

        public int CaloriesPerServing { get; init; }

        public int ReadyIn { get; init; }

        public string ImageUrl { get; init; }
    }
}
