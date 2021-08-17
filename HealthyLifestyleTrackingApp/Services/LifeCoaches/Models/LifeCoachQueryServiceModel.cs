using System.Collections.Generic;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches.Models
{
    public class LifeCoachQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int LifeCoachesPerPage { get; init; }

        public int TotalLifeCoaches { get; init; }

        public IEnumerable<LifeCoachServiceModel> LifeCoaches { get; init; }
    }
}
