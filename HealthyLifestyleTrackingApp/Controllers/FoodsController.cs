﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Models.Foods;
using HealthyLifestyleTrackingApp.Services.Foods;
using static HealthyLifestyleTrackingApp.WebConstants;
using Microsoft.AspNetCore.Authorization;
using HealthyLifestyleTrackingApp.Infrastructure;

namespace HealthyLifestyleTrackingApp.Controllers
{
    public class FoodsController : Controller
    {
        private readonly IFoodService foods;

        public FoodsController(IFoodService foods)
        {
            this.foods = foods;
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

            var foodCategories = this.foods.GetFoodCategories().Select(c => c.Name).ToList();

            var foodTags = this.foods.GetFoodTags().Select(c => c.Name).ToList();

            query.Categories = foodCategories;
            query.Tags = foodTags;
            query.TotalFoods = queryResult.TotalFoods;
            query.Foods = queryResult.Foods;

            return View(query);
        }

        public IActionResult Details(int id, string information)
        {
            var food = this.foods.Details(id);

            if (!information.Contains(food.Name))
            {
                return BadRequest();
            }

            return View(food); 
        }

        public IActionResult Create() => View(new CreateFoodFormModel
        {
            FoodCategories = this.foods.GetFoodCategories(),
            Tags = this.foods.GetFoodTags()
        });

        [HttpPost]
        public IActionResult Create(CreateFoodFormModel food)
        {
            if (!this.foods.FoodCategoryExists(food.FoodCategoryId))
            {
                this.ModelState.AddModelError(nameof(food.FoodCategoryId), "Food category does not exist.");
            }


            if (!this.foods.StandardServingTypeExists((int)food.StandardServingType))
            {
                this.ModelState.AddModelError(nameof(food.StandardServingType), "Serving type does not exist.");
            }


            if (food.FoodTags != null)
            {
                foreach (var tag in food.FoodTags)
                {
                    if (!this.foods.FoodTagsExists(tag))
                    {
                        this.ModelState.AddModelError(nameof(food.FoodTags), "Food tag does not exist.");
                    }

                }
            }

            if (!ModelState.IsValid)
            {
                food.FoodCategories = this.foods.GetFoodCategories();
                food.Tags = this.foods.GetFoodTags();
                return View(food);
            }

            var foodId = this.foods.Create(
                food.Name,
                food.Brand,
                (double)food.StandardServingAmount,
                food.StandardServingType,
                food.ImageUrl,
                (int)food.Calories,
                (double)food.Protein,
                (double)food.Carbohydrates,
                (double)food.Fat,
                food.FoodCategoryId,
                food.FoodTags);

            TempData[GlobalMessageKey] = "Successfully created food!";

            return RedirectToAction(nameof(Details), new { id = foodId, information = food.Name });
        }

        public IActionResult Track()
           => View();

        [HttpPost]
        [Authorize]
        public IActionResult Track(int id, string information, TrackFoodFormModel food)
        {
            var foodName = this.foods.GetFoodName(id);

            if (!information.Contains(foodName))
            {
                return BadRequest();
            }

            if (!this.foods.MealTypeExists((int)food.MealType))
            {
                this.ModelState.AddModelError(nameof(food.MealType), "Meal type does not exist.");
            }

            var userId = this.User.GetId();

            if (userId == null)
            {
                return Redirect("~/Identity/Account/Login");
            }
            
            if (id == 0)
            {
                return BadRequest();
            }


            if (!ModelState.IsValid)
            {
                return View(food);
            }

            var trackedFoodId = this.foods.Track(
                id,
                userId,
                food.AmountInGrams,
                food.MealType);

            return RedirectToAction(nameof(All));
        }
    }
}
