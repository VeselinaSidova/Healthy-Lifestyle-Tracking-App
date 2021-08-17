using System.Linq;
using HealthyLifestyleTrackingApp.Data;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Services.LifeCoaches.Models;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public class LifeCoachService : ILifeCoachService
    {
        private readonly HealthyLifestyleTrackerDbContext data;

        public LifeCoachService(HealthyLifestyleTrackerDbContext data)
            => this.data = data;

        public LifeCoachQueryServiceModel All(
            int currentPage = 1,
            int lifeCoachesPerPage = 10,
            bool approvedOnly = true)
        {
            var lifeCoachesQuery = this.data.LifeCoaches
                .Where(c => approvedOnly ? c.IsApprovedLifeCoach : true);

            var totalLifeCoaches = lifeCoachesQuery.Count();

            var lifeCoaches = lifeCoachesQuery
                .OrderBy(c => c.FirstName)
                .ThenBy(c => c.LastName)
                .Skip((currentPage - 1) * lifeCoachesPerPage)
                .Take(lifeCoachesPerPage)
                .Select(c => new LifeCoachServiceModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ProfilePictureUrl = c.ProfilePictureUrl,
                    About = c.About,
                    IsApprovedLifeCoach = c.IsApprovedLifeCoach
                })
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
                UserId = userId,
                IsApprovedLifeCoach = false
            };

            this.data.LifeCoaches.Add(lifeCoachData);
            this.data.SaveChanges();

            return lifeCoachData.Id;
        }

        public void ApproveForLifeCoach(int id)
        {
            var lifeCoach = this.data.LifeCoaches.Where(c => c.Id == id).FirstOrDefault();

            lifeCoach.IsApprovedLifeCoach = !lifeCoach.IsApprovedLifeCoach;

            this.data.SaveChanges();
        }

        public void DeleteApplication(int id)
        {
            var lifeCoach = this.data.LifeCoaches.Where(c => c.Id == id).FirstOrDefault();

            this.data.Remove(lifeCoach);

            this.data.SaveChanges();
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
            .Where(c => c.IsApprovedLifeCoach == true)
            .Any(c => c.UserId == userId);
    }
}
