using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;

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

        public IEnumerable<string> AllFoodCategories()
            => this.data.FoodCategories.Select(c => c.Name).OrderBy(c => c).Distinct().ToList();

        public IEnumerable<string> AllFoodTags()
            => this.data.Tags.Select(t => t.Name).ToList();
    }
}
