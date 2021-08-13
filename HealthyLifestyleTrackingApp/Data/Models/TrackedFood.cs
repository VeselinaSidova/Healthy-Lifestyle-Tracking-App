using System;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Data.Enums;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class TrackedFood
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        public Food Food { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public int TrackerId { get; set; }

        public Tracker Tracker { get; set; }

        public double AmountInGrams { get; set; }

        public MealType MealType { get; set; }

        public DateTime DateTracked { get; set; }       
    }
}
