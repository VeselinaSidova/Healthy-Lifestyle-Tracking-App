using System;

namespace HealthyLifestyleTrackingApp.Services.Tracker.Models
{
    public class TrackedExerciseServiceModel
    {
        public int Id { get; init; }

        public int ExerciseId { get; init; }

        public string ExerciseName { get; init; }

        public int Duration { get; init; }

        public double Calories { get; set; }

        public string ImageUrl { get; init; }

        public DateTime DateTracked { get; init; }
    }
}
