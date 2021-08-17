using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Services.LifeCoaches.Models;

namespace HealthyLifestyleTrackingApp.Models.LifeCoaches
{
    public class AllLifeCoachesQueryModel
    {
        public const int LifeCoachesPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        public int TotalLifeCoaches { get; set; }

        public IEnumerable<LifeCoachServiceModel> LifeCoaches { get; set; }
    }
}
