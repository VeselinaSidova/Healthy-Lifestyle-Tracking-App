using HealthyLifestyleTrackingApp.Data.Enums;
using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Foods.Models
{
    public class FoodDetailsServiceModel : FoodServiceModel
    {
        public double StandardServingAmount { get; init; }

        public StandardServingType StandardServingType { get; init; }

        public double Protein { get; init; }

        public double Carbohydrates { get; init; }

        public double Fat { get; init; }

        public int FoodCategoryId { get; init; }

        public IEnumerable<string> Tags { get; init; }

        public ICollection<int> FoodTags { get; init; }
    }
}
