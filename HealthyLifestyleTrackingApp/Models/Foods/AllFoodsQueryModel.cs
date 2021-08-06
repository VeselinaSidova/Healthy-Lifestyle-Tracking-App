using HealthyLifestyleTrackingApp.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Models.Foods
{
    public class AllFoodsQueryModel
    {
        public const int FoodsPerPage = 8;

        public string Category { get; init; }

        public string Tag { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public Sorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalFoods { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<FoodListingViewModel> Foods { get; set; }
    }
}
