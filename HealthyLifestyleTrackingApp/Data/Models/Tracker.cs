using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class Tracker
    {
        public int Id { get; init; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<TrackedFood> TrackedFoods { get; set; } = new List<TrackedFood>();

        public IEnumerable<TrackedExercise> TrackedExercises { get; set; } = new List<TrackedExercise>();
    }
}
