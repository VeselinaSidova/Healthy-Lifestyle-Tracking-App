using System;
using HealthyLifestyleTrackingApp.Data.Enums;

namespace HealthyLifestyleTrackingApp.Services.Tracker.Models
{
    public class TrackedFoodServiceModel
    {
        public int Id { get; init; }

        public int FoodId { get; init; }

        public string FoodName { get; init; }

        public double AmountInGrams { get; init; }

        public double Calories { get; set; }

        public string ImageUrl { get; init; }

        public MealType MealType { get; init; }

        public DateTime DateTracked { get; init; }
    }
}
