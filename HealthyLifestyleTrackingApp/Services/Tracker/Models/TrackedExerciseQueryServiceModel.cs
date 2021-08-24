using System;
using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Tracker.Models
{
    public class TrackedExerciseQueryServiceModel
    {
        public DateTime DateTracked { get; set; }

        public IEnumerable<TrackedExerciseServiceModel> Exercises { get; init; }
    }
}
