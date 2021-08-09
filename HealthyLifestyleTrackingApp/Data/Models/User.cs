using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static HealthyLifestyleTrackingApp.Data.DataConstants.User;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }
    }
}
