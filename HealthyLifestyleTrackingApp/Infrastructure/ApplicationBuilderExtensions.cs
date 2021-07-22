using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyLifestyleTrackingApp.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<HealthyLifestyleTrackerDbContext>();

            data.Database.Migrate();

            SeesFoodCategories(data);
            SeesExerciseCategories(data);

            return app;
        }

        private static void SeesFoodCategories(HealthyLifestyleTrackerDbContext data)
        {
            if (data.FoodCategories.Any())
            {
                return;
            }

            data.FoodCategories.AddRange(new[]
            {
                new FoodCategory { Name = "Bread and Bakery" },
                new FoodCategory { Name = "Meat" },
                new FoodCategory { Name = "Dairy" },
                new FoodCategory { Name = "Baverages" },
                new FoodCategory { Name = "Fish" },
                new FoodCategory { Name = "Fruits" },
                new FoodCategory { Name = "Nuts" },
                new FoodCategory { Name = "Ready meals" },
                new FoodCategory { Name = "Snacks and Desserts" },
                new FoodCategory { Name = "Vegetables" },
            });

            data.SaveChanges();
        }

        private static void SeesExerciseCategories(HealthyLifestyleTrackerDbContext data)
        {
            if (data.ExerciseCategories.Any())
            {
                return;
            }

            data.ExerciseCategories.AddRange(new[]
            {
                new ExerciseCategory { Name = "Light" },
                new ExerciseCategory { Name = "Moderate" },
                new ExerciseCategory { Name = "High Intensity" },
                new ExerciseCategory { Name = "Strenght Training" },
            });

            data.SaveChanges();
        }
    }
}
