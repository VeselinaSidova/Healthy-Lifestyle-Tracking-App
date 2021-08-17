namespace HealthyLifestyleTrackingApp.Services.Recipes.Models
{
    public class RecipeDetailsServiceModel : RecipeServiceModel
    {
        public string Instructions { get; init; }

        public int LifeCoachId { get; init; }

        public string UserId { get; init; }
    }
}
