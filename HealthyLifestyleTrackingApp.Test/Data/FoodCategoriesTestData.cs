using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data.Models;

namespace HealthyLifestyleTrackingApp.Test.Data
{
    public class FoodCategoriesTestData
    {
        public static List<FoodCategory> GetFoodCategories(int count)
        {
            var foodCategories = Enumerable
                .Range(1, count)
                .Select(i => new FoodCategory
                {
                    Id = i,
                    Name = $"Category {i}"
                })
                .ToList();

            return foodCategories;
        }
    }
}
