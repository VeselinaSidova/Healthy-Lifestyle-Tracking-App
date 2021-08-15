namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public class FoodServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Brand { get; init; }

        public string ImageUrl { get; init; }

        public int Calories { get; init; }

        public string FoodCategory { get; init; }
    }
}
