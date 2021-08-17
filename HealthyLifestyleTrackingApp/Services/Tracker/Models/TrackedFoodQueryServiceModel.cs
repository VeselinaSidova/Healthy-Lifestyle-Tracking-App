using System;
using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.Tracker.Models
{
    public class TrackedFoodQueryServiceModel
    {
        public DateTime DateTracked { get; set; }

        public IEnumerable<TrackedFoodServiceModel> Foods { get; init; }
    }
}
