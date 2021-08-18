using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Services.Articles;
using HealthyLifestyleTrackingApp.Services.Exercises;
using HealthyLifestyleTrackingApp.Services.Foods;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using Microsoft.AspNetCore.Authentication.Cookies;
using HealthyLifestyleTrackingApp.Services.Recipes;
using HealthyLifestyleTrackingApp.Services.Tracker;

namespace HealthyLifestyleTrackingApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;


        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<HealthyLifestyleTrackerDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HealthyLifestyleTrackerDbContext>();

            services.AddMemoryCache();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<IFoodService, FoodService>();
            services.AddTransient<IExerciseService, ExerciseService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<ILifeCoachService, LifeCoachService>();
            services.AddTransient<ITrackerService, TrackerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "Areas",
                        pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                      name: "Article Read",
                      pattern: "/Article/Read/{id}/{information}",
                      defaults: new { controller = "Articles", action = "Read" });
                    endpoints.MapControllerRoute(
                       name: "Article Edit",
                       pattern: "/Article/Edit/{id}/{information}",
                       defaults: new { controller = "Articles", action = "Edit" });
                    endpoints.MapControllerRoute(
                        name: "Food Details",
                        pattern: "/Food/Details/{id}/{information}",
                        defaults: new { controller = "Foods", action = "Details" });
                    endpoints.MapControllerRoute(
                       name: "Food Edit",
                       pattern: "/Food/Edit/{id}/{information}",
                       defaults: new { controller = "Foods", action = "Edit" });
                    endpoints.MapControllerRoute(
                       name: "Food Track",
                       pattern: "/Food/Track/{id}/{information}",
                       defaults: new { controller = "Foods", action = "Track" });
                    endpoints.MapControllerRoute(
                      name: "Exercise Edit",
                      pattern: "/Exercise/Edit/{id}/{information}",
                      defaults: new { controller = "Exercises", action = "Edit" });
                    endpoints.MapControllerRoute(
                       name: "Exercise Track",
                       pattern: "/Exercise/Track/{id}/{information}",
                       defaults: new { controller = "Exercises", action = "Track" });
                    endpoints.MapControllerRoute(
                      name: "Recipe Read",
                      pattern: "/Recipe/Read/{id}/{information}",
                      defaults: new { controller = "Recipes", action = "Read" });
                    endpoints.MapControllerRoute(
                       name: "Recipe Edit",
                       pattern: "/Recipe/Edit/{id}/{information}",
                       defaults: new { controller = "Recipes", action = "Edit" });
                    endpoints.MapControllerRoute(
                       name: "View Tracked",
                       pattern: "/Tracker/ViewTracked/{selectedDateString}",
                       defaults: new { controller = "Tracker", action = "ViewTracked" });
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
