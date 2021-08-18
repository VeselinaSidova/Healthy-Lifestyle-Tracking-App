﻿using System.Linq;
using Xunit;
using MyTested.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.LifeCoaches;

using static HealthyLifestyleTrackingApp.WebConstants;

namespace HealthyLifestyleTrackingApp.Test.Pipeline
{
    public class LifeCoachesControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndReturnView()
           => MyPipeline
               .Configuration()
               .ShouldMap(request => request
                   .WithPath("/LifeCoaches/Become")
                   .WithUser())
               .To<LifeCoachesController>(c => c.Become())
               .Which()
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View();

        [Theory]
        [InlineData("Test", "Testov", "https://testphoto.com/photo.jpg", "This is a test about field that should be at least one hundred characters long. It explains the about information so he can be approved by admin.")]
        public void PostBecomeBeForAuthirizedUsersAndReturnRedirectWithValidModel(
            string firstName,
            string lastName,
            string profilePictureUrl,
            string about)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/LifeCoaches/Become")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        ProfilePictureUrl = profilePictureUrl,
                        About = about
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<LifeCoachesController>(c => c.Become(new BecomeLifeCoachFormModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    ProfilePictureUrl = profilePictureUrl,
                    About = about
                }))
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<LifeCoach>(lifeCoaches => lifeCoaches
                    .Any(c =>
                         c.FirstName == firstName &&
                         c.LastName == lastName &&
                         c.ProfilePictureUrl == profilePictureUrl &&
                         c.About == about &&
                         c.UserId == TestUser.Identifier)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<LifeCoachesController>(c => c.All(With.Any<AllLifeCoachesQueryModel>())));
    }
}
