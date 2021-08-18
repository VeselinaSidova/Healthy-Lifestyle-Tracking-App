using System.Linq;
using Xunit;
using MyTested.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Test.Data;

using static HealthyLifestyleTrackingApp.WebConstants;


namespace HealthyLifestyleTrackingApp.Test.Controllers
{

    public class ArticlesControllerTest
    {
        [Fact]
        public void GetAllArticlesShouldBeForAuthorizedUsersAndReturnView()
                => MyController<ArticlesController>
                     .Instance(instance => instance
                      .WithUser())
                     .Calling(c => c.All(With.Default<AllArticlesQueryModel>()))
                     .ShouldHave()
                        .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
                     .AndAlso()
                     .ShouldReturn()
                     .View(view => view
                         .WithModelOfType<AllArticlesQueryModel>());

        [Fact]
        public void GetMineArticlesShouldBeForAuthorizedUsersAndReturnMineView()
                => MyController<ArticlesController>
                     .Instance(instance => instance
                        .WithData(Articles.GetArticles(1))
                        .WithUser(user => user.WithIdentifier("testId")))
                     .Calling(c => c.Mine())
                     .ShouldHave()
                        .ActionAttributes(attributes => attributes
                        .RestrictingForAuthorizedRequests())
                     .AndAlso()
                     .ShouldReturn()
                     .View();


        [Fact]
        public void GetCreateShouldBeForAuthorizedUsersAndLifeCoachesAndReturnView()
                => MyController<ArticlesController>
                      .Instance(instance => instance
                        .WithData(LifeCoaches.GetLifeCoaches(1, true))
                      .WithUser(user => user.WithIdentifier("testId")))
                      .Calling(c => c.Create())
                      .ShouldHave()
                      .ActionAttributes(attributes => attributes
                          .RestrictingForAuthorizedRequests())
                      .AndAlso()
                      .ShouldReturn()
                      .View();

        [Fact]
        public void GetCreateShouldBeForAuthorizedUsersAndShouldRedirectToBecomeLifeCoachIfUserIsNotLifeCoach()
                  => MyController<ArticlesController>
                      .Instance(instance => instance
                      .WithUser())
                        .Calling(c => c.Create())
                       .ShouldHave()
                       .ActionAttributes(attributes => attributes
                           .RestrictingForAuthorizedRequests())
                      .AndAlso()
                      .ShouldReturn()
                      .Redirect(redirect => redirect
                           .To<LifeCoachesController>(c => c.Become()));


        [Fact]
        public void PostCreateShouldReturnViewWithSameModelWhenInvalidModelState()
                   => MyController<ArticlesController>
                        .Instance(instance => instance
                             .WithData(LifeCoaches.GetLifeCoaches(1, true))
                             .WithUser(user => user.WithIdentifier("testId")))
                       .Calling(c => c.Create(With.Default<ArticleFormModel>()))
                       .ShouldHave()
                       .InvalidModelState()
                       .AndAlso()
                       .ShouldReturn()
                       .View(With.Default<ArticleFormModel>());


        [Theory]
        [InlineData("Article Title", "Article Content for this article must be at least one hundred characters long so that is the number of characters written.", "https://testphoto.com/photo.jpg")]
        public void CreatePostShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModel(string title, string content, string imageUrl)
           => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(LifeCoaches.GetLifeCoaches(1, true))
                    .WithUser(user => user.WithIdentifier("testId")))
               .Calling(c => c.Create(new ArticleFormModel
               {
                   Title = title,
                   Content = content,
                   ImageUrl = imageUrl,
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldHave()
               .Data(data => data
                    .WithSet<Article>(articles => articles
                    .Any(a =>
                         a.Title == title &&
                         a.Content == content &&
                         a.ImageUrl == imageUrl &&
                         a.LifeCoachId == 1)))
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<ArticlesController>(c => c.Mine()));
    }
}
