namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class FoodListingViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Brand { get; init; }

        public string ImageUrl { get; init; }

        public int Calories { get; init; }

        public double Protein { get; init; }

        public double Carbohydrates { get; init; }

        public double Fat { get; init; }

        public string FoodCategory { get; init; }
    }
}
