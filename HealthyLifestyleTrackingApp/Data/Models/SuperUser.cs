using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.SuperUser;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class SuperUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        public string UserId { get; set; }

        public int LifeCoachId { get; set; }

        public LifeCoach LifeCoach { get; set; }
    }
}
