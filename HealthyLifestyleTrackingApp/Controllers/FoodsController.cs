using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Foods;
using HealthyLifestyleTrackingApp.Services.Foods;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class FoodsController : Controller
    {
        private readonly IFoodService foods;
        private readonly HealthyLifestyleTrackerDbContext data;

        public FoodsController(IFoodService foods, HealthyLifestyleTrackerDbContext data)
        {
            this.foods = foods;
            this.data = data;
        }

        public IActionResult All([FromQuery] AllFoodsQueryModel query)
        {
            var queryResult = this.foods.All(
                query.Category,
                query.Tag,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllFoodsQueryModel.FoodsPerPage);

            var foodCategories = this.foods.AllFoodCategories();

            var foodTags = this.foods.AllFoodTags();

            query.Categories = foodCategories;
            query.Tags = foodTags;
            query.TotalFoods = queryResult.TotalFoods;
            query.Foods = queryResult.Foods;

            return View(query);
        }

        public IActionResult Create() => View(new CreateFoodFormModel
        {
            FoodCategories = this.GetFoodCategories(),
            Tags = this.GetFoodTags()
        });

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
           .OrderBy(t => t.Name)
           .ToList();
    }
}
