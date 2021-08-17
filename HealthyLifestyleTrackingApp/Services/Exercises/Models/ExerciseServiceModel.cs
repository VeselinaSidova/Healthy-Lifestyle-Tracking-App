namespace HealthyLifestyleTrackingApp.Services.Exercises.Models
{
    public class ExerciseServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int CaloriesPerHour { get; init; }

        public string ImageUrl { get; init; }

        public string ExerciseCategory { get; init; }
    }
}
