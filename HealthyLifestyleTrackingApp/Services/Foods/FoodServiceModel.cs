namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public class FoodServiceModel
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string ImageUrl { get; set; }

        public int Calories { get; set; }

        public string FoodCategory { get; init; }
    }
}
