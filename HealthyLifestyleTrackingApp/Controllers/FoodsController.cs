using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Foods;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult Create(CreateFoodFormModel food)
        {
            if (!this.data.FoodCategories.Any(c => c.Id == food.FoodCategoryId))
            {
                this.ModelState.AddModelError(nameof(food.FoodCategoryId), "Food category does not exist.");
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

            return RedirectToAction("Index", "Home");
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
