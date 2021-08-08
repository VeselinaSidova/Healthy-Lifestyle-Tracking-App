using System.Linq;
using HealthyLifestyleTrackingApp.Data;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public class LifeCoachService : ILifeCoachService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public LifeCoachService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public bool IsLifeCoach(string userId)
            => this.data
            .LifeCoaches
            .Any(c => c.UserId == userId);
    }
}
