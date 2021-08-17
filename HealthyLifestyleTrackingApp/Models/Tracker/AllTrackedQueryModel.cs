using System;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Services.Tracker.Models;

namespace HealthyLifestyleTrackingApp.Models.Tracker
{
    public class AllTrackedQueryModel
    {
        public DateTime DateTracked { get; set; }

        public IEnumerable<TrackedFoodServiceModel> Foods { get; set; }

        public IEnumerable<TrackedExerciseServiceModel> Exercises { get; set; }
    }
}
