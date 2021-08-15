using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.LifeCoaches;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public class LifeCoachService : ILifeCoachService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public LifeCoachService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public LifeCoachQueryServiceModel All(int currentPage, int lifeCoachesPerPage)
        {
            var lifeCoachesQuery = this.data.LifeCoaches.AsQueryable();

            var totalLifeCoaches = lifeCoachesQuery.Count();

            var lifeCoaches = lifeCoachesQuery
                .Skip((currentPage - 1) * lifeCoachesPerPage)
                .Take(lifeCoachesPerPage)
                .Select(c => new LifeCoachServiceModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ProfilePictureUrl = c.ProfilePictureUrl,
                    About = c.About
                })
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .ToList();

            return new LifeCoachQueryServiceModel
            {
                CurrentPage = currentPage,
                LifeCoachesPerPage = lifeCoachesPerPage,
                TotalLifeCoaches = totalLifeCoaches,
                LifeCoaches = lifeCoaches
            };
        }

        public int Become(
            string firstName, 
            string lastName, 
            string profilePictureUrl, 
            string about, 
            string userId)
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
