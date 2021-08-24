using System;
using System.Linq;
using System.Threading.Tasks;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<HealthyLifestyleTrackerDbContext>();

            data.Database.Migrate();

            SeedFoodCategories(data);
            SeedExerciseCategories(data);
            SeedTags(data);
            SeedAdministrator(serviceProvider);

            return app;
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
               .Run(async () =>
               {
                   if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                   {
                       return;
                   }
                   var administratorRole = new IdentityRole { Name = AdministratorRoleName }; 

                   await roleManager.CreateAsync(administratorRole);

                   const string adminEmail = "admin@healthtracker.com";
                   const string adminPassword = "admin1234";

                   var user = new User
                   {
                       Email = adminEmail,
                       UserName = adminEmail,
                       FirstName = "Veselina",
                       LastName = "Sidova (Admin)"
                   };

                   await userManager.CreateAsync(user, adminPassword);

                   await userManager.AddToRoleAsync(user, administratorRole.Name);
               })
               .GetAwaiter()
               .GetResult();
           
        }

        private static void SeedFoodCategories(HealthyLifestyleTrackerDbContext data)
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

        private static void SeedExerciseCategories(HealthyLifestyleTrackerDbContext data)
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
                new ExerciseCategory { Name = "Strength Training" },
            });

            data.SaveChanges();
        }


        private static void SeedTags(HealthyLifestyleTrackerDbContext data)
        {
            if (data.Tags.Any())
            {
                return;
            }

            data.Tags.AddRange(new[]
            {
                new Tag { Name = "Healthy" },
                new Tag { Name = "Rich in Protein" },
                new Tag { Name = "Rich in Carbs" },
                new Tag { Name = "Rich in Fat" },
                new Tag { Name = "Junk Food " },
                new Tag { Name = "Alcohol" },
                new Tag { Name = "Vegeterian" },
                new Tag { Name = "Vegan" },
                new Tag { Name = "Low in Calories"},
                new Tag { Name = "Dense in Calories"},
                new Tag { Name = "High in Sugar"},
            });

            data.SaveChanges();
        }
    }
}
