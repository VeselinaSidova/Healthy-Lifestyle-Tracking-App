using System;
using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Service.Foods;

namespace HealthyLifestyleTrackingApp.Services.Foods
{
    public class FoodService : IFoodService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public FoodService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public FoodQueryServiceModel All(
            string category, 
            string tag, 
            string searchTerm, 
            Sorting sorting, 
            int currentPage, 
            int foodsPerPage)
        {
            var foodsQuery = this.data.Foods.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                foodsQuery = foodsQuery.Where(f => f.FoodCategory.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                foodsQuery = foodsQuery.Where(f => f.FoodTags.Any(t => t.Tag.Name == tag));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                foodsQuery = foodsQuery.Where(f =>
                    f.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    f.Brand.ToLower().Contains(searchTerm.ToLower()));
            }

            foodsQuery = sorting switch
            {
                Sorting.Category => foodsQuery.OrderBy(f => f.FoodCategory.Name),
                Sorting.DateCreated => foodsQuery.OrderByDescending(f => f.Id),
                Sorting.Name or _ => foodsQuery.OrderBy(f => f.Name)
            };

            var totalFoods = foodsQuery.Count();

            var foods = foodsQuery
                .Skip((currentPage - 1) * foodsPerPage)
                .Take(foodsPerPage)
                .Select(f => new FoodServiceModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Brand = f.Brand,
                    ImageUrl = f.ImageUrl,
                    Calories = f.Calories,
                    FoodCategory = f.FoodCategory.Name
                })
                .ToList();

            return new FoodQueryServiceModel
            {
                CurrentPage = currentPage,
                TotalFoods = totalFoods,
                FoodsPerPage = foodsPerPage,
                Foods = foods
            };
        }

        public FoodDetailsServiceModel Details(int id)
        {
            var food = this.data
                .Foods
                .Where(f => f.Id == id)
                .Select(f => new FoodDetailsServiceModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Brand = f.Brand,
                    ImageUrl = f.ImageUrl,
                    FoodCategory = f.FoodCategory.Name,
                    StandardServingAmount = f.StandardServingAmount,
                    StandardServingType = f.StandardServingType,
                    Protein = f.Protein,
                    Carbohydrates = f.Carbohydrates,
                    Fat = f.Fat,
                    Tags = f.FoodTags.Select(t => t.Tag.Name).ToList(),
                })
                .FirstOrDefault();

            return food;
        }

        public int Create(
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
            ICollection<int> foodTags)
        {
            var foodData = new Food
            {
                Name = name,
                Brand = brand,
                StandardServingAmount = (double)standardServingAmount,
                StandardServingType = standardServingType,
                ImageUrl = imageUrl,
                Calories = (int)calories,
                Protein = (double)protein,
                Carbohydrates = (double)carbohydrates,
                Fat = (double)fat,
                FoodCategoryId = foodCategoryId,
            };
            if (foodTags != null)
            {
                foreach (var foodTag in foodTags)
                {
                    var tag = data.Tags.FirstOrDefault(x => x.Id == foodTag)
                        ?? new Tag { Id = foodTag };
                    foodData.FoodTags.Add(new FoodTag { Tag = tag });
                }
            }
            

            this.data.Foods.Add(foodData);
            this.data.SaveChanges();

            return foodData.Id;
        }

        public bool FoodCategoryExists(int categoryId)
            => this.data.FoodCategories.Any(c => c.Id == categoryId);


        public bool StandardServingTypeExists(int standardServingTypeInt)
            => Enum.IsDefined(typeof(StandardServingType), standardServingTypeInt);

        public bool FoodTagsExists(int tag)
            => this.data.Tags.Any(t => t.Id == tag);


        public IEnumerable<FoodCategoryServiceModel> GetFoodCategories()
         => this.data
         .FoodCategories
         .Select(c => new FoodCategoryServiceModel
         {
             Id = c.Id,
             Name = c.Name
         })
         .ToList();

        public IEnumerable<FoodTagServiceModel> GetFoodTags()
           => this.data
           .Tags
           .Select(t => new FoodTagServiceModel
           {
               Id = t.Id,
               Name = t.Name
           })
           .OrderBy(t => t.Name)
           .ToList();
    }
}
