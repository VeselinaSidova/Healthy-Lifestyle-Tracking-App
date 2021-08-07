using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data.Enums;

namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public interface IFoodService
    {
        FoodQueryServiceModel All(
            string category, 
            string tag, 
            string searchTerm, 
            Sorting sorting, 
            int currentPage, 
            int foodsPerPage);

        IEnumerable<string> AllFoodCategories();

        IEnumerable<string> AllFoodTags();
    }
}
