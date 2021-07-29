using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class CreateFoodFormModel
    {
        public string Name { get; init; }

        public string Brand { get; init; }

        public double Amount { get; init; }

        [EnumDataType(typeof(ServingType))]
        [Display(Name = "Serving Type")]
        public ServingType ServingType { get; init; }

        public int Calories { get; init; }

        public double Protein { get; init; }

        public double Carbohydrates { get; init; }

        public double Fat { get; init; }

        [Display(Name = "Category")]
        public int FoodCategoryId { get; init; }

        public IEnumerable<FoodCategoryViewModel> FoodCategories { get; set; }

        public IEnumerable<FoodTagViewModel> Tags { get; set; }

        public ICollection<FoodTag> FoodTags { get; set; }
    }
}
