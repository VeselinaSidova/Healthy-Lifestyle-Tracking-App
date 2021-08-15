using HealthyLifestyleTrackingApp.Models.LifeCoaches;

namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public interface ILifeCoachService
    {
        LifeCoachQueryServiceModel All(
            int currentPage, 
            int lifeCoachesPerPage);

        int Become(
            string firstName,
            string lastName,
            string profilePictureUrl,
            string about,
            string userId);

        public bool IsLifeCoach(string userId);

        public int GetIdByUser(string userId);
    }
}
