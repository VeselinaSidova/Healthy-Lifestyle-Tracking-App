using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;

namespace HealthyLifestyleTrackingApp.Test.Data
{
    public static class FoodsTestData
    {
        public static List<Food> GetFoods(int count)
        {
            var foodCategories = FoodCategoriesTestData.GetFoodCategories(1);

            var foods = Enumerable
                .Range(1, count)
                .Select(i => new Food
                {
                    Id = i,
                    Name = $"Name {i}",
                    Brand = $"Brand {i}",
                    StandardServingAmount = i + 10,
                    StandardServingType = (StandardServingType)2,
                    FoodCategoryId = i,
                    FoodCategory = foodCategories[i - 1],
                    ImageUrl = $"https://testphoto.com/photo.jpg",
                    Calories = i + 10,
                    Protein = i,
                    Carbohydrates = i,
                    Fat = i,
                    FoodTags = null
                })
                .ToList();

            return foods;
        }
    }
}
