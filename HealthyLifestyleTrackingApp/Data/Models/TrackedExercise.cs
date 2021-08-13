using System;
using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class TrackedExercise
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public int TrackerId { get; set; }

        public Tracker Tracker { get; set; }

        public int ExerciseId { get; set; }

        public Exercise Exercise { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime DateTracked { get; set; }
    }
}
