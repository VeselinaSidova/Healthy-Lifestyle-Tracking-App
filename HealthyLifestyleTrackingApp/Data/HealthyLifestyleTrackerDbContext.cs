using HealthyLifestyleTrackingApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace HealthyLifestyleTrackingApp.Data
{
    public class HealthyLifestyleTrackerDbContext : IdentityDbContext<User>
    {
        public HealthyLifestyleTrackerDbContext(DbContextOptions<HealthyLifestyleTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Food> Foods { get; init; }
        public DbSet<Exercise> Exercises { get; init; }
        public DbSet<FoodCategory> FoodCategories { get; init; }
        public DbSet<ExerciseCategory> ExerciseCategories { get; init; }
        public DbSet<Tag> Tags { get; init; }
        public DbSet<FoodTag> FoodTags { get; init; }
        public DbSet<Article> Articles { get; init; }
        public DbSet<Recipe> Recipes { get; init; }
        public DbSet<LifeCoach> LifeCoaches { get; init; }
        public DbSet<SuperUser> SuperUsers { get; init; }


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

            builder
                .Entity<FoodTag>()
                .HasKey(x => new { x.FoodId, x.TagId });

            builder
                .Entity<FoodTag>()
                .HasOne(b => b.Food)
                .WithMany(b => b.FoodTags)
                .HasForeignKey(b => b.FoodId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<FoodTag>()
                .HasOne(b => b.Tag)
                .WithMany(b => b.FoodTags)
                .HasForeignKey(b => b.TagId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Article>()
                .HasOne(a => a.LifeCoach)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.LifeCoachId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Recipe>()
                .HasOne(a => a.LifeCoach)
                .WithMany(c => c.Recipes)
                .HasForeignKey(a => a.LifeCoachId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<SuperUser>()
                .HasOne(a => a.LifeCoach)
                .WithMany(c => c.SuperUsers)
                .HasForeignKey(a => a.LifeCoachId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<LifeCoach>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<LifeCoach>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<SuperUser>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<SuperUser>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
