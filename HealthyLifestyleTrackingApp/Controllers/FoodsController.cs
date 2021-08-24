using System.Linq;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Models.Foods;
using HealthyLifestyleTrackingApp.Services.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static HealthyLifestyleTrackingApp.WebConstants;

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
            var food = this.foods.Details(id, information);

            if (food == null)
            {
                return NotFound("Food not found!");
            }

            return View(food); 
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new FoodFormModel
            {
                FoodCategories = this.foods.GetFoodCategories(),
                Tags = this.foods.GetFoodTags()
            });
        }
              

        [HttpPost]
        [Authorize]
        public IActionResult Create(FoodFormModel food)
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

            TempData[GlobalMessageKey] = "Food was successfully created.";

            return RedirectToAction(nameof(Details), new { id = foodId, information = food.Name });
        }


        [Authorize]
        public IActionResult Edit(int id, string information)
        {
            if (!User.IsAdmin())
            {
                return Unauthorized("Only admin can edit foods!");
            }

            var food = this.foods.Details(id, information);

            if (food == null)
            {
                return NotFound("Food not found!");
            }

            return View(new FoodFormModel
            {
                Name = food.Name,
                Brand = food.Brand,
                StandardServingAmount = food.StandardServingAmount,
                StandardServingType = food.StandardServingType,
                ImageUrl = food.ImageUrl,
                Calories = food.Calories,
                Protein = food.Protein,
                Carbohydrates = food.Carbohydrates,
                Fat = food.Fat,
                FoodCategoryId = food.FoodCategoryId,
                FoodCategories = this.foods.GetFoodCategories(),
                Tags = this.foods.GetFoodTags(),
                FoodTags = food.FoodTags
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, FoodFormModel food)
        {
            if (!this.foods.FoodExists(id))
            {
                return NotFound("Food not found");
            }

            if (!User.IsAdmin())
            {
                return Unauthorized("Only admin can edit foods!");
            }

            if (!ModelState.IsValid)
            {
                return View(food);
            }

            var foodIsEdited = foods.Edit(
               id,
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

            if (foodIsEdited == false)
            {
                return BadRequest("Food was not edited!");
            }

            TempData[GlobalMessageKey] = "Food was successfully edited.";

            return RedirectToAction(nameof(Details), new { id = id, information = food.Name });
        }


        [Authorize]
        public IActionResult Track()
        {
           return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Track(int id, string information, TrackFoodFormModel food)
        {
            var userId = this.User.GetId();

            if (userId == null)
            {
                return Redirect("~/Identity/Account/Login");
            }

            var foodName = this.foods.GetFoodName(id);

            if (!information.Contains(foodName))
            {
                return BadRequest();
            }

            if (!this.foods.MealTypeExists((int)food.MealType))
            {
                this.ModelState.AddModelError(nameof(food.MealType), "Meal type does not exist.");
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

            TempData[GlobalMessageKey] = "Food was added to tracker.";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!User.IsAdmin())
            {
                return BadRequest();
            }

            this.foods.Delete(id);

            return RedirectToAction(nameof(All));
        }
    }
}
