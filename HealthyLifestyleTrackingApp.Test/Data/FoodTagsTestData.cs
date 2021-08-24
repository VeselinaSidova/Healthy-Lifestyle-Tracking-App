using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data.Models;

namespace HealthyLifestyleTrackingApp.Test.Data
{
    class FoodTagsTestData
    {
        public static List<Tag> GetTags(int count)
        {
            var tags = Enumerable
                .Range(1, count)
                .Select(i => new Tag
                {
                    Id = i,
                    Name = $"Tag {i}",
                })
                .ToList();

            return tags;
        }
    }
}
