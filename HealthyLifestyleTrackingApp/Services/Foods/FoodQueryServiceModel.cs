using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public class FoodQueryServiceModel
    {
        
        public int CurrentPage { get; init; }

        public int FoodsPerPage { get; init; }

        public int TotalFoods { get; set; }

        public IEnumerable<FoodServiceModel> Foods { get; init; }
    }
}
