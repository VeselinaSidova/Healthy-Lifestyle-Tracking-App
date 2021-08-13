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

        FoodDetailsServiceModel Details(int id);

        int Create(
                string name,
                string brand,
                double standardServingAmount,
                StandardServingType standardServingType,
                string imageUrl,
                int calories,
                double protein,
                double carbohydrates,
                double fat,
                int foodCategoryId,
                ICollection<int> foodTags);

        IEnumerable<FoodCategoryServiceModel> GetFoodCategories();

        IEnumerable<FoodTagServiceModel> GetFoodTags();

        bool FoodCategoryExists(int categoryId);

        bool StandardServingTypeExists(int standardServingTypeInt);

        bool FoodTagsExists(int tag);
    }
}
