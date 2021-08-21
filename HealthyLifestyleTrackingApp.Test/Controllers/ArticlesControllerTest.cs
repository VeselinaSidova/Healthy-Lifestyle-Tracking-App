using System.Linq;
using System.Collections.Generic;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Services.Articles.Models;
using HealthyLifestyleTrackingApp.Test.Data;
using MyTested.AspNetCore.Mvc;
using Xunit;

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
        public void GetMineArticlesShouldBeForAuthorizedUsersAndReturnView()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(Articles.GetArticles(1, false))
                    .WithUser(user => user.WithIdentifier("userId1")))
               .Calling(c => c.Mine())
               .ShouldHave()
                  .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View();


        [Fact]
        public void MineShouldReturnViewWithCorrectModel()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(Articles.GetArticles(2, sameUser: false))
                    .WithUser(user => user.WithIdentifier("userId1")))
                .Calling(c => c.Mine())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<List<ArticleServiceModel>>()
                    .Passing(articles =>
                    {
                        articles.Any();
                        articles.SingleOrDefault(a => a.Author == "FirstName LastName").Equals(1);
                    }));


        [Fact]
        public void GetReadShouldReturnNotFoundWhenInvalidArticleIdAndInvalidName()
            => MyController<ArticlesController>
               .Calling(c => c.Read(int.MaxValue, "Name"))
               .ShouldReturn()
               .NotFound();
          

        [Fact]
        public void GetReadShouldReturnBadRequestWhenValidArticleIdAndInvalidName()
            => MyController<ArticlesController>
               .Instance(instance => instance
                    .WithData(Articles.GetArticles(1)))
               .Calling(c => c.Read(1, "Name"))
               .ShouldReturn()
               .BadRequest();


        [Fact]
        public void GetReadShouldBeForAuthorizedUsersAndShouldReturnCorrectArticle()
           => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(Articles.GetArticles(1)))
                .Calling(c => c.Read(1, "Article 1"))
                .ShouldHave()
                    .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleDetailsServiceModel>()
                    .Passing(article =>
                    {
                        article.Id.Equals(1);
                        article.Title.Equals("Article 1");
                    }));


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
        public void GetCreateShouldBeForAuthorizedUsersAndShouldRedirectToBecomeLifeCoachIfUserIsNotApprovedLifeCoach()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(LifeCoaches.GetLifeCoaches(1, false))
                    .WithUser(user => user.WithIdentifier("userId1")))
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<LifeCoachesController>(c => c.Become()));


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
        public void PostCreateShouldBeAllowedOnlyForPostRequestsAndAuthorizedUsers()
            => MyController<ArticlesController>
            .Instance(instance => instance
                .WithUser())
            .Calling(c => c.Create(With.Default<ArticleFormModel>()))
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests()
                .RestrictingForHttpMethod(HttpMethod.Post));


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
        public void PostCreateShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModel(string title, string content, string imageUrl)
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

        [Fact]
        public void EditGetShouldBeAvailableForAuthorizedUsersAuthorsOnly()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(Articles.GetArticles(1, false))
                    .WithUser(user => user.WithIdentifier("userId1")))
                .Calling(c => c.Edit(1))
                .ShouldHave()
                .ActionAttributes(attrs => attrs
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArticleFormModel>()
                    .Passing(article =>
                    {
                        article.Title.Equals("Article 1");
                        article.Content.Equals("Article Content for this article must be at least 100 characters long so that is the number of characters written. 1");
                        article.ImageUrl.Equals("https://testphoto.com/photo.jpg");
                    }));


        [Fact]
        public void EditGetShouldReturnNotFoundWhenInvalidId()
           => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(Articles.GetArticles(1, false))
                    .WithUser(user => user.WithIdentifier("userId1")))
               .Calling(c => c.Edit(int.MaxValue))
               .ShouldReturn()
               .NotFound();
    }
}
