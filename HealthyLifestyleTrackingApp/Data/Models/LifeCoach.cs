using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static HealthyLifestyleTrackingApp.Data.DataConstants.LifeCoach;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class LifeCoach
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ProfilePictureUrl { get; set; }

        [Required]
        [MaxLength(AboutMaxLength)]
        public string About { get; set; }

        public bool IsApprovedLifeCoach { get; set; }

        public IEnumerable<Article> Articles { get; set; } = new List<Article>();

        public IEnumerable<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
