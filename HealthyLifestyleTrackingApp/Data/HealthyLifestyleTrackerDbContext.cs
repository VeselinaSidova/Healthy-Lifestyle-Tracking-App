using HealthyLifestyleTrackingApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HealthyLifestyleTrackingApp.Data
{
    public class HealthyLifestyleTrackerDbContext : IdentityDbContext
    {
        public HealthyLifestyleTrackerDbContext(DbContextOptions<HealthyLifestyleTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Food> Foods { get; init; }
        public DbSet<Exercise> Exercises { get; init; }
        public DbSet<FoodCategory> FoodCategories { get; init; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Food>()
                .HasOne(c => c.FoodCategory)
                .WithMany(c => c.Foods)
                .HasForeignKey(c => c.FoodCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Exercise>()
                .HasOne(c => c.ExerciseCategory)
                .WithMany(c => c.Exercises)
                .HasForeignKey(c => c.ExerciseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
