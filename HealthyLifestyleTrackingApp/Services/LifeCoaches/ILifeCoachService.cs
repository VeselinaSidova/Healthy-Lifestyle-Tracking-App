namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public interface ILifeCoachService
    {
        int Create(
            string firstName,
            string lastName,
            string profilePictureUrl,
            string userId);

        public bool IsLifeCoach(string userId);

        public int GetIdByUser(string userId);
    }
}
