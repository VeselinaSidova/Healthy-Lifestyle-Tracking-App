using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Services.Foods.Models;

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

        int Track( 
            int foodId, 
            string userId, 
            double amountInGrams, 
            MealType mealType);

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

        public bool Edit(
           int foodId,
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

        void Delete(int id);

        IEnumerable<FoodCategoryServiceModel> GetFoodCategories();

        IEnumerable<FoodTagServiceModel> GetFoodTags();

        string GetFoodName(int foodId);

        bool FoodCategoryExists(int categoryId);

        bool StandardServingTypeExists(int standardServingTypeInt);

        bool FoodTagsExists(int tag);

        bool MealTypeExists(int mealType);
    }
}
