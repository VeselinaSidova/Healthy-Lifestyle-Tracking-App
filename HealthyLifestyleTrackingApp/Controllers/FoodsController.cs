using HealthyLifestyleTrackingApp.Data;
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
            return View();
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
