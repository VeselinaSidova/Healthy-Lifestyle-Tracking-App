using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Foods;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class FoodsController : Controller
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public FoodsController(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public IActionResult Create() => View(new CreateFoodFormModel
        {
            FoodCategories = this.GetFoodCategories(),
            Tags = this.GetFoodTags()
        });

        public IActionResult All([FromQuery]AllFoodsQueryModel query)
        {
            var foodsQuery = this.data.Foods.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                foodsQuery = foodsQuery.Where(f => f.FoodCategory.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                foodsQuery = foodsQuery.Where(f => f.FoodTags.Any(t => t.Tag.Name == query.Tag));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                foodsQuery = foodsQuery.Where(f =>
                    f.Name.ToLower().Contains(query.SearchTerm.ToLower()) || 
                    f.Brand.ToLower().Contains(query.SearchTerm.ToLower()) || 
                    f.FoodCategory.Name.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            foodsQuery = query.Sorting switch
            {
                FoodSorting.Category => foodsQuery.OrderBy(f => f.FoodCategory.Name),
                FoodSorting.DateCreated => foodsQuery.OrderByDescending(f => f.Id),
                FoodSorting.Name or _ => foodsQuery.OrderBy(f => f.Name)
            };

            var foods = foodsQuery
                .Select(f => new FoodListingViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Brand = f.Brand,
                    ImageUrl = f.ImageUrl,
                    Calories = f.Calories,
                    Protein = f.Protein,
                    Carbohydrates = f.Carbohydrates,
                    Fat = f.Fat,
                    FoodCategory = f.FoodCategory.Name
                })
                .ToList();

            var foodCategories = this.data.FoodCategories.Select(c => c.Name).OrderBy(c => c).Distinct().ToList();

            var foodTags = this.data.Tags.Select(t => t.Name).ToList();

            query.Categories = foodCategories;
            query.Foods = foods;
            query.Tags = foodTags;

            return View(query);
        }

        [HttpPost]
        public IActionResult Create(CreateFoodFormModel food)
        {
            if (!this.data.FoodCategories.Any(c => c.Id == food.FoodCategoryId))
            {
                this.ModelState.AddModelError(nameof(food.FoodCategoryId), "Food category does not exist.");
            }


            if (!Enum.IsDefined(typeof(StandardServingType), (int)food.StandardServingType))
            {
                this.ModelState.AddModelError(nameof(food.StandardServingType), "Serving type does not exist.");
            }

            var tagIds = this.data.Tags.Select(x => x.Id).ToList();

            if (food.FoodTags != null)
            {
                foreach (var tag in food.FoodTags)
                {
                    if (!this.data.Tags.Any(t => t.Id == tag))
                    {
                        this.ModelState.AddModelError(nameof(food.FoodTags), "Food tag does not exist.");
                    }
                   
                }
            }


            if (!ModelState.IsValid)
            {
                food.FoodCategories = this.GetFoodCategories();
                food.Tags = this.GetFoodTags();
                return View(food);
            }

            var foodData = new Food
            {
                Name = food.Name,
                Brand = food.Brand,
                StandardServingAmount = (double)food.StandardServingAmount,
                StandardServingType = food.StandardServingType,
                ImageUrl = food.ImageUrl,
                Calories = (int)food.Calories,
                Protein = (double)food.Protein,
                Carbohydrates = (double)food.Carbohydrates,
                Fat = (double)food.Fat,
                FoodCategoryId = food.FoodCategoryId,
            };
            foreach (var foodTag in food.FoodTags)
            {
                var tag = data.Tags.FirstOrDefault(x => x.Id == foodTag)
                    ?? new Tag { Id = foodTag };
                foodData.FoodTags.Add(new FoodTag { Tag = tag });
            }

            this.data.Foods.Add(foodData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<FoodCategoryViewModel> GetFoodCategories()
            => this.data
            .FoodCategories
            .Select(c => new FoodCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();


        private IEnumerable<FoodTagViewModel> GetFoodTags()
           => this.data
           .Tags
           .Select(t => new FoodTagViewModel
           {
               Id = t.Id,
               Name = t.Name
           })
           .ToList();
    }
}
