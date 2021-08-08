using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Service.Foods;

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

        //IEnumerable<string> AllFoodCategories();

        //IEnumerable<string> AllFoodTags();

        IEnumerable<FoodCategoryServiceModel> GetFoodCategories();

        IEnumerable<FoodTagServiceModel> GetFoodTags();
    }
}
