using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Infrastructure;
using HealthyLifestyleTrackingApp.Services.Articles;
using HealthyLifestyleTrackingApp.Services.Exercises;
using HealthyLifestyleTrackingApp.Services.Foods;
using HealthyLifestyleTrackingApp.Services.LifeCoaches;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<HealthyLifestyleTrackerDbContext>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });

            services.AddTransient<IFoodService, FoodService>();
            services.AddTransient<IExerciseService, ExerciseService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ILifeCoachService, LifeCoachService>();
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
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapRazorPages();
                });
        }
    }
}
