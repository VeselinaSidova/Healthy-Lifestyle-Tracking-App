namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public interface ILifeCoachService
    {
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
