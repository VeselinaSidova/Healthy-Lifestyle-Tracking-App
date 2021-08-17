using HealthyLifestyleTrackingApp.Services.LifeCoaches.Models;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public interface ILifeCoachService
    {
        LifeCoachQueryServiceModel All(
            int currentPage = 1,
            int lifeCoachesPerPage = 10,
            bool approvedOnly = true);

        int Become(
            string firstName,
            string lastName,
            string profilePictureUrl,
            string about,
            string userId);

        void ApproveForLifeCoach(int id);

        void DeleteApplication(int id);

        public bool IsLifeCoach(string userId);

        public int GetIdByUser(string userId);
    }
}
