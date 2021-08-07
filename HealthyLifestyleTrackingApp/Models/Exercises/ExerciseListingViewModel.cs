namespace HealthyLifestyleTrackingApp.Models.Exercises
{
    public class ExerciseListingViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int CaloriesPerHour { get; init; }

        public string ImageUrl { get; init; }

        public int ExerciseCategoryId { get; init; }

        public string ExerciseCategory { get; init; }
    }
}
