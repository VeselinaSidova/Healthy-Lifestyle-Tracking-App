using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

using static HealthyLifestyleTrackingApp.Data.DataConstants.User;

namespace HealthyLifestyleTrackingApp.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(NameMaxLength)]
        public string LastName { get; set; }

        public IEnumerable<TrackedFood> TrackedFoods { get; set; } = new List<TrackedFood>();

        public IEnumerable<TrackedExercise> TrackedExercises { get; set; } = new List<TrackedExercise>();

        public static explicit operator User(LifeCoach v)
        {
            throw new NotImplementedException();
        }
    }
}
