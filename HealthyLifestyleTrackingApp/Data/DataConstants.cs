namespace HealthyLifestyleTrackingApp.Data
{
    public class DataConstants
    {
        public class Food
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
            public const int BrandMinLength = 1;
            public const int BrandMaxLength = 20;
            public const double StandardServingAmountMinValue = 0.1;
            public const double StandardServingAmountMaxValue = 5000;
            public const int CaloriesMinValue = 0;
            public const int CaloriesMaxValue = 10000;
            public const double NutritionMinValue = 0;
            public const double NutritionMaxValue = 100;
        }

        public class Exercise 
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
            public const int CaloriesPerHourMinValue = 1;
            public const int CaloriesPerHourMaxValue = 10000;
        }

        public class Category 
        {
            public const int NameMaxLenghth = 20;
        } 

        public class FoodTag
        {
            public const int NameMaxLength = 20;
        }

        public class Recipe
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
            public const int ServingsCountMinValue = 1;
            public const int ServingsCountMaxValue = 20;
            public const int CaloriesPerServingMinValue = 0;
            public const int CaloriesPerServingMaxValue = 10000;
            public const int ReadyInMinValue = 1;
            public const int ReadyInMaxValue = 1000;
            public const int InstructionsMinLength = 50;
            public const int InstructionsMaxLength = 10000;
        }

        public class Article
        {
            public const int TitleMaxLength = 50;
            public const int TitleMinLength = 2;
            public const int ContentMinLength = 50;
            public const int ContentMaxLength = 50000;
        }

        public class LifeCoach
        {
            public const int NameMinLength = 1;
            public const int NameMaxLength = 30;
            public const int AboutMinLength = 50;
            public const int AboutMaxLength = 1000;
        }

        public class User
        {
            public const int NameMinLength = 1;
            public const int NameMaxLength = 30;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class TrackedFood
        {
            public const double AmountMinValue = 0.1;
            public const double AmountMaxValue = 5000;
        }

        public class TrackedExercise
        {
            public const int DurationMinValue = 1;
            public const int DurationMaxValue = 1000;
        }
    }
}
