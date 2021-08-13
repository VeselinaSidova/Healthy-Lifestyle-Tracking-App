using HealthyLifestyleTrackingApp.Data.Enums;
using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public class FoodDetailsServiceModel : FoodServiceModel
    {
        public double StandardServingAmount { get; init; }

        public StandardServingType StandardServingType { get; init; }

        public double Protein { get; init; }

        public double Carbohydrates { get; init; }

        public double Fat { get; init; }

        public IEnumerable<string> Tags { get; init; }
    }
}
