using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.Exercise;

namespace HealthyLifestyleTrackingApp.Models.LifeCoaches
{
    public class BecomeLifeCoachFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "First name should be between 1 and 30 characters long.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Last name should be between 1 and 30 characters long.")]
        public string LastName { get; set; }

        [Display(Name = "Profile Picture URL")]
        [Required]
        [Url]
        public string ProfilePictureUrl { get; set; }
    }
}
