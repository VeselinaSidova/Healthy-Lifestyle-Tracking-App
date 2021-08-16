namespace HealthyLifestyleTrackingApp.Services.LifeCoaches
{
    public class LifeCoachServiceModel
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string ProfilePictureUrl { get; init; }

        public string About { get; init; }

        public bool IsApprovedLifeCoach { get; init; }
    }
}
