namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class FoodTag
    {
        public int FoodId { get; init; }

        public Food Food { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
