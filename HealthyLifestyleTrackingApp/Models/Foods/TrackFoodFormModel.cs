using System;
using System.ComponentModel.DataAnnotations;
using HealthyLifestyleTrackingApp.Data.Enums;

using static HealthyLifestyleTrackingApp.Data.DataConstants.TrackedFood;

namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class TrackFoodFormModel
    {
        [Required]
        [Display(Name = "Amount In Grams")]
        [Range(AmountMinValue, AmountMaxValue, ErrorMessage = "Value should be between 0.1 and 5000 grams.")]
        public double AmountInGrams { get; set; }

        [Required]
        [Display(Name = "Meal Type")]
        public MealType MealType { get; set; }

        [Required]

        public DateTime DateTracked { get; set; }
    }
}
