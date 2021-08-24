using System;
using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Data.Models;
using MyTested.AspNetCore.Mvc;


namespace HealthyLifestyleTrackingApp.Test.Data
{
    public class ArticlesTestData
    {
        public static List<Article> GetArticles(int count, bool sameUser = true)
        {
            var user = new LifeCoach
            {
                UserId = TestUser.Identifier
            };

            var articles = Enumerable
                .Range(1, count)
                .Select(i => new Article
                {
                    Id = i,
                    Title = $"Article {i}",
                    Content = $"Article Content for this article must be at least 100 characters long so that is the number of characters written. {i}",
                    ImageUrl = $"https://testphoto.com/photo.jpg",
                    CreatedOn = new DateTime(2021, 8, 17),
                    LifeCoachId = i,
                    LifeCoach = sameUser ? user : new LifeCoach
                    {
                        Id = i,
                        UserId = $"userId{i}",
                        FirstName = "FirstName",
                        LastName = "LastName"
                    }
                })
                .ToList();

            return articles;
        }
    }
}
