using System;
using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Services.Foods.Models;

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

        public FoodDetailsServiceModel Details(int id, string information)
        {
            var food = this.data
                .Foods
                .Where(f => f.Id == id && f.Name == information)
                .Select(f => new FoodDetailsServiceModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Brand = f.Brand,
                    ImageUrl = f.ImageUrl,
                    FoodCategory = f.FoodCategory.Name,
                    FoodCategoryId = f.FoodCategoryId,
                    StandardServingAmount = f.StandardServingAmount,
                    StandardServingType = f.StandardServingType,
                    Calories = f.Calories,
                    Protein = f.Protein,
                    Carbohydrates = f.Carbohydrates,
                    Fat = f.Fat,
                    Tags = f.FoodTags.Select(t => t.Tag.Name).ToList(),
                    FoodTags = f.FoodTags.Select(t => t.Tag.Id).ToList()
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
            ICollection<int> foodTags)
        {
            var foodData = this.data.Foods.Find(foodId);

            if (foodData == null)
            {
                return false;
            }

            foodData.Name = name;
            foodData.Brand = brand;
            foodData.StandardServingAmount = standardServingAmount;
            foodData.StandardServingType = standardServingType;
            foodData.ImageUrl = imageUrl;
            foodData.Calories = calories;
            foodData.Carbohydrates = carbohydrates;
            foodData.Fat = fat;
            foodData.FoodCategoryId = foodCategoryId;

            var foodTagsToRemove = this.data.FoodTags.Where(t => t.FoodId == foodId).ToList();
            if (foodTagsToRemove.Count() != 0)
            {
                this.data.RemoveRange(foodTagsToRemove);
                this.data.SaveChanges();
            }
            
            if (foodTags != null)
            {
                foreach (var foodTag in foodTags)
                {
                    var tag = data.Tags.FirstOrDefault(x => x.Id == foodTag)
                        ?? new Tag { Id = foodTag };
                    foodData.FoodTags.Add(new FoodTag { Tag = tag });
                }
            }
            this.data.SaveChanges();

            return true;
        }


        public int Track(
            int foodId, 
            string userId, 
            double amountInGrams, 
            MealType mealType)
        {
            var trackedFood = new TrackedFood
            {
                FoodId = foodId,
                UserId = userId,
                AmountInGrams = amountInGrams,
                MealType = mealType,
                DateTracked = DateTime.Now
            };

            this.data.TrackedFoods.Add(trackedFood);
            this.data.SaveChanges();

            return trackedFood.Id;
        }

        public void Delete(int id)
        {
            var foodTags = this.data.FoodTags.Where(t => t.FoodId == id).ToList();

            var food = this.data.Foods.Where(c => c.Id == id).FirstOrDefault();

            this.data.RemoveRange(foodTags);
            this.data.Remove(food);

            this.data.SaveChanges();
        }


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

        public string GetFoodName(int foodId)
        {
            var food = this.data.Foods.Where(f => f.Id == foodId).FirstOrDefault();
            return food.Name;
        }


        public bool FoodExists(int foodId)
            => this.data.Foods.Any(f => f.Id == foodId);


        public bool FoodCategoryExists(int categoryId)
            => this.data.FoodCategories.Any(c => c.Id == categoryId);

        public bool StandardServingTypeExists(int standardServingTypeInt)
            => Enum.IsDefined(typeof(StandardServingType), standardServingTypeInt);

        public bool FoodTagsExists(int tag)
            => this.data.Tags.Any(t => t.Id == tag);

        public bool MealTypeExists(int mealTypeInt)
           => Enum.IsDefined(typeof(MealType), mealTypeInt);
    }
}
