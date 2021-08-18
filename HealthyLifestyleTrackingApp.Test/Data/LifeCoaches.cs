using HealthyLifestyleTrackingApp.Data.Models;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyLifestyleTrackingApp.Test.Data
{
    public class LifeCoaches
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
                    FirstName = "FirstName",
                    LastName = "LastName",
                    IsApprovedLifeCoach = isApproved,
                    UserId = "testId",
                })
                .ToList();

            return lifeCoaches;
        }
    }
}
