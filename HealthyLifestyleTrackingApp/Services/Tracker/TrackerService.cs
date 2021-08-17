using System;
using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Services.Tracker.Models;

namespace HealthyLifestyleTrackingApp.Services.Tracker
{
    public class TrackerService : ITrackerService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public TrackerService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public TrackedFoodQueryServiceModel AllTrackedFoods(
            DateTime selectedDate,
            string userId)
        {
            var foodsQuery = this.data.TrackedFoods.Where(f => f.UserId == userId && f.DateTracked.Date == selectedDate).AsQueryable();

            var foods = foodsQuery
                .Select(f => new TrackedFoodServiceModel
                {
                    Id = f.Id,
                    FoodId = f.FoodId,
                    FoodName = f.Food.Name,
                    AmountInGrams = f.AmountInGrams,
                    Calories = f.AmountInGrams * (double)f.Food.Calories / 100,
                    ImageUrl = f.Food.ImageUrl,
                    MealType = f.MealType,
                    DateTracked = f.DateTracked
                })
                .ToList();

            return new TrackedFoodQueryServiceModel
            {
                DateTracked = selectedDate,
                Foods = foods
            };
        }

        public TrackedExerciseQueryServiceModel AllTrackedExercises(
            DateTime selectedDate,
            string userId)
        {
            var exerciseQuery = this.data.TrackedExercises.Where(f => f.UserId == userId && f.DateTracked.Date == selectedDate).AsQueryable();

            var exercises = exerciseQuery
                .Select(f => new TrackedExerciseServiceModel
                {
                    Id = f.Id,
                    ExerciseId = f.ExerciseId,
                    ExerciseName = f.Exercise.Name,
                    Duration = (int)f.Duration.TotalMinutes,
                    Calories = (double)f.Exercise.CaloriesPerHour / 60 * f.Duration.TotalMinutes,
                    ImageUrl = f.Exercise.ImageUrl,
                    DateTracked = f.DateTracked
                })
                .ToList();

            return new TrackedExerciseQueryServiceModel
            {
                DateTracked = selectedDate,
                Exercises = exercises
            };
        }

        public void DeleteTrackedFood(int id, string userId)
        {
            var trackedFoods = this.data.TrackedFoods.Where(f => f.UserId == userId).ToList();

            var foodToDelete = trackedFoods.Where(f => f.Id == id).FirstOrDefault();

            this.data.Remove(foodToDelete);

            this.data.SaveChanges();
        }

        public void DeleteTrackedExercise(int id, string userId)
        {
            var trackedExercises = this.data.TrackedExercises.Where(f => f.UserId == userId).ToList();

            var exerciseToDelete = trackedExercises.Where(f => f.Id == id).FirstOrDefault();

            this.data.Remove(exerciseToDelete);

            this.data.SaveChanges();
        }
    }
}
