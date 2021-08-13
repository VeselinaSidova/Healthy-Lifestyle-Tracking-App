using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public class LifeCoachService : ILifeCoachService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public LifeCoachService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public int Create(string firstName, string lastName, string profilePictureUrl, string about, string userId)
        {
            var lifeCoachData = new LifeCoach
            {
                FirstName = firstName,
                LastName = lastName,
                ProfilePictureUrl = profilePictureUrl,
                About = about,
                UserId = userId
            };

            this.data.LifeCoaches.Add(lifeCoachData);
            this.data.SaveChanges();

            return lifeCoachData.Id;
        }

        public int GetIdByUser(string userId)
            => this.data
                .LifeCoaches
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .FirstOrDefault();

        public bool IsLifeCoach(string userId)
            => this.data
            .LifeCoaches
            .Any(c => c.UserId == userId);
    }
}
