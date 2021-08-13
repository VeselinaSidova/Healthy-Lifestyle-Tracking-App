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
        public DbSet<Tracker> Trackers { get; init; }
        public DbSet<TrackedFood> TrackedFoods { get; init; }
        public DbSet<TrackedExercise> TrackedExercises { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Food>()
                .HasOne(f => f.FoodCategory)
                .WithMany(c => c.Foods)
                .HasForeignKey(f => f.FoodCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Exercise>()
                .HasOne(e => e.ExerciseCategory)
                .WithMany(c => c.Exercises)
                .HasForeignKey(e => e.ExerciseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<FoodTag>()
                .HasKey(t => new { t.FoodId, t.TagId });

            builder
                .Entity<FoodTag>()
                .HasOne(t => t.Food)
                .WithMany(f => f.FoodTags)
                .HasForeignKey(t => t.FoodId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<FoodTag>()
                .HasOne(t => t.Tag)
                .WithMany(t => t.FoodTags)
                .HasForeignKey(t => t.TagId)
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
                .HasOne(r => r.LifeCoach)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.LifeCoachId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<LifeCoach>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<LifeCoach>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<TrackedFood>()
                .HasOne(f => f.Food)
                .WithMany(f => f.TrackedFoods)
                .HasForeignKey(f => f.FoodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<TrackedExercise>()
               .HasOne(e => e.Exercise)
               .WithMany(e => e.TrackedExercises)
               .HasForeignKey(e => e.ExerciseId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<TrackedFood>()
                .HasOne(f => f.User)
                .WithMany(u => u.TrackedFoods)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .Entity<TrackedExercise>()
              .HasOne(e => e.User)
              .WithMany(u => u.TrackedExercises)
              .HasForeignKey(e => e.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<TrackedFood>()
                .HasOne(f => f.Tracker)
                .WithMany(t => t.TrackedFoods)
                .HasForeignKey(f => f.TrackerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
             .Entity<TrackedExercise>()
             .HasOne(e => e.Tracker)
             .WithMany(t => t.TrackedExercises)
             .HasForeignKey(e => e.TrackerId)
             .OnDelete(DeleteBehavior.Restrict);

            builder
             .Entity<User>()
             .HasOne(u => u.Tracker)
             .WithOne(t => t.User)
             .HasForeignKey<Tracker>(u => u.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>().ToTable("Users");

            builder.Entity<Tracker>().ToTable("Trackers");

            base.OnModelCreating(builder);
        }
    }
}

