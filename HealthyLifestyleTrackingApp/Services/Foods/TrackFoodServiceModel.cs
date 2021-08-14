using System;
using HealthyLifestyleTrackingApp.Data.Enums;

namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public class TrackFoodServiceModel : FoodDetailsServiceModel
    {
        public double AmountInGrams { get; set; }

        public MealType MealType { get; set; }

        public DateTime DateTracked { get; set; }
    }
}
