using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data.Models;
using MyTested.AspNetCore.Mvc;

namespace HealthyLifestyleTrackingApp.Test.Data
{
    public class LifeCoachesTestData
    {
        public static List<LifeCoach> GetLifeCoaches(int count, bool isApproved = true)
        {
            var user = new User
            {
                Id = TestUser.Identifier,
                UserName = TestUser.Username
            };

            var lifeCoaches = Enumerable
                .Range(1, count)
                .Select(i => new LifeCoach
                {
                    Id = i,
                    IsApprovedLifeCoach = isApproved,
                    UserId = isApproved ? "testId" : "userId{i}"
                })
                .ToList();

            return lifeCoaches;
        }
    }
}
