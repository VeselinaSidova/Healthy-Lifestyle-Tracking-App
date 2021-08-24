using System;
using HealthyLifestyleTrackingApp.Services.Tracker.Models;

namespace HealthyLifestyleTrackingApp.Services.Tracker
{
    public interface ITrackerService
    {
        TrackedFoodQueryServiceModel AllTrackedFoods(
            DateTime selectedDate, 
            string userId);

        TrackedExerciseQueryServiceModel AllTrackedExercises(
            DateTime selectedDate,
            string userId);

        void DeleteTrackedFood(int id, string userId);

        void DeleteTrackedExercise(int id, string userId);
    }
}
