using System.Collections.Generic;
using System.Linq;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Models;
using HealthyLifestyleTrackingApp.Models.Articles;
using HealthyLifestyleTrackingApp.Services.Articles.Models;
using HealthyLifestyleTrackingApp.Test.Data;
using MyTested.AspNetCore.Mvc;
using Shouldly;
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
                    .WithData(ArticlesTestData.GetArticles(1, false))
                    .WithUser(user => user.WithIdentifier("userId1")))
               .Calling(c => c.Mine())
               .ShouldHave()
                  .ActionAttributes(attributes => attributes
                  .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View();


        [Fact]
        public void GetMineShouldReturnViewWithCorrectModel()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticlesTestData.GetArticles(2, sameUser: false))
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
                    .WithData(ArticlesTestData.GetArticles(1)))
               .Calling(c => c.Read(1, "Name"))
               .ShouldReturn()
               .BadRequest();


        [Fact]
        public void GetReadShouldBeForAuthorizedUsersAndShouldReturnCorrectArticle()
           => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticlesTestData.GetArticles(1)))
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
                    .WithData(LifeCoachesTestData.GetLifeCoaches(1, true))
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
                    .WithData(LifeCoachesTestData.GetLifeCoaches(1, false))
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
                    .WithData(LifeCoachesTestData.GetLifeCoaches(1, true))
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
                    .WithData(LifeCoachesTestData.GetLifeCoaches(1, true))
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
        public void GetEditShouldBeAvailableForAuthorizedUsersAndAuthorsOnly()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticlesTestData.GetArticles(1, false))
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
        public void GetEditShouldBeAvailableForAdmins()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticlesTestData.GetArticles(1))
                    .WithUser(user => user.InRole("Administator")))
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
        public void GetEditShouldReturnNotFoundWhenInvalidId()
           => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(ArticlesTestData.GetArticles(1, false))
                    .WithUser(user => user.WithIdentifier("userId1")))
               .Calling(c => c.Edit(int.MaxValue))
               .ShouldReturn()
               .NotFound();


        [Fact]
        public void GetEditShouldReturnUnathorizedWhenNonAuthorTriesToEdit()
          => MyController<ArticlesController>
              .Instance(instance => instance
                  .WithUser(user => user.WithIdentifier("userId"))
                  .WithData(ArticlesTestData.GetArticles(1)))
              .Calling(c => c.Edit(1))
              .ShouldReturn()
              .Unauthorized();


        [Fact]
        public void PostEditShouldHaveRestrictionsForHttpPostAndAuthorizedUsers()
           => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(LifeCoachesTestData.GetLifeCoaches(1, true))
                    .WithUser(user => user.WithIdentifier("testId")))
               .Calling(c => c.Edit(
                   With.Empty<int>(),
                   With.Empty<ArticleFormModel>()))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForHttpMethod(HttpMethod.Post)
                   .RestrictingForAuthorizedRequests());


        [Fact]
        public void PostEditShouldReturnNotFoundWhenInvalidId()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithData(LifeCoachesTestData.GetLifeCoaches(1, true))
                    .WithUser(user => user.WithIdentifier("testId")))
                .Calling(c => c.Edit(
                    With.Any<int>(),
                    With.Any<ArticleFormModel>()))
                .ShouldReturn()
                .NotFound();


        [Fact]
        public void PostEditShouldReturUnauthorizedWhenNonAuthorTriesToEdit()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithUser(user => user.WithIdentifier("NonAuthor"))
                    .WithData(ArticlesTestData.GetArticles(1)))
                .Calling(c => c.Edit(1, With.Empty<ArticleFormModel>()))
                .ShouldReturn()
                .Unauthorized();


        [Fact]
        public void PostEditShouldReturnViewWithSameModelWhenInvalidModelState()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithUser()
                    .WithData(ArticlesTestData.GetArticles(1)))
                .Calling(c => c.Edit(1, With.Default<ArticleFormModel>()))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View(With.Default<ArticleFormModel>());


        [Fact]
        public void PostEditShouldReturnViewWhenAdminTriesToEdit()
           => MyController<ArticlesController>
               .Instance(instance => instance
                   .WithUser(user => user.InRole("Administrator"))
                   .WithData(ArticlesTestData.GetArticles(1)))
               .Calling(c => c.Edit(1, With.Empty<ArticleFormModel>()))
               .ShouldReturn()
                 .Redirect(redirect => redirect
                   .To<ArticlesController>(c => c.All(With.Empty<AllArticlesQueryModel>())));




        [Theory]
        [InlineData(1, "Article Title", "Article Content for this article must be at least 100 characters long so that is the number of characters written.", "https://testphoto.com/photo.jpg", TestUser.Username, null)]
        public void PostEditShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModelStateWhenAuthorEdits(
            int articleId,
            string title,
            string content,
            string imageUrl,
            string username,
            string role)
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithUser(username, new[] { role })
                    .WithData(ArticlesTestData.GetArticles(1)))
                .Calling(c => c.Edit(articleId, new ArticleFormModel
                {
                    Title = $"Edit {title}",
                    Content = $"Edit {content}",
                    ImageUrl = $"{imageUrl}/test"
                }))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(data => data
                    .WithSet<Article>(set =>
                    {
                        set.Any();
                        var article = set.SingleOrDefault(a => a.Id == articleId);
                        article.ShouldNotBeNull();
                        article.Title.Equals($"Edit {title}");
                        article.Content.Equals($"Edit {content}");
                        article.ImageUrl.Equals($"{imageUrl}/test");
                    }))
                .AndAlso()
                .ShouldHave()
                .TempData(tempData => tempData
                    .ContainingEntryWithKey(GlobalMessageKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<ArticlesController>(c => c.Mine()));


        [Theory]
        [InlineData(1, "Article Title", "Article Content for this article must be at least 100 characters long so that is the number of characters written here.", "https://testphoto.com/photo.jpg", "Administrator", WebConstants.AdministratorRoleName)]
        public void PostEditShouldSaveArticleSetTempDataMessageAndRedirectWhenValidModelStateWhenAdminEdits(
           int articleId,
           string title,
           string content,
           string imageUrl,
           string username,
           string role)
           => MyController<ArticlesController>
               .Instance(instance => instance
                   .WithUser(username, new[] { role })
                   .WithData(ArticlesTestData.GetArticles(1)))
               .Calling(c => c.Edit(articleId, new ArticleFormModel
               {
                   Title = $"Edit {title}",
                   Content = $"Edit {content}",
                   ImageUrl = $"{imageUrl}/test"
               }))
               .ShouldHave()
               .ValidModelState()
               .AndAlso()
               .ShouldHave()
               .Data(data => data
                   .WithSet<Article>(set =>
                   {
                       set.Any();
                       var article = set.SingleOrDefault(a => a.Id == articleId);
                       article.ShouldNotBeNull();
                       article.Title.Equals($"Edit {title}");
                       article.Content.Equals($"Edit {content}");
                       article.ImageUrl.Equals($"{imageUrl}/test");
                   }))
               .AndAlso()
               .ShouldHave()
               .TempData(tempData => tempData
                   .ContainingEntryWithKey(GlobalMessageKey))
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<ArticlesController>(c => c.All(With.Empty<AllArticlesQueryModel>())));


        [Fact]
        public void GetDeleteShouldOnlyBeAllowedForAuthorizedUsers()
           => MyController<ArticlesController>
             .Instance(instance => instance
                    .WithUser())
               .Calling(c => c.Delete(With.Empty<int>()))
               .ShouldHave()
               .ActionAttributes(attrs => attrs
                   .RestrictingForAuthorizedRequests());

        [Fact]
        public void GetDeleteShouldReturnNotFoundWhenInvalidId()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithUser(user => user.WithIdentifier("testId"))
                    .WithData(ArticlesTestData.GetArticles(1)))
                .Calling(c => c.Delete(With.Any<int>()))
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void GetDeleteShouldReturnNotFoundWhenNonAuthorUser()
            => MyController<ArticlesController>
                .Instance(instance => instance
                    .WithUser(user => user.WithIdentifier("NonAuthor"))
                    .WithData(ArticlesTestData.GetArticles(1)))
                .Calling(c => c.Delete(1))
                .ShouldReturn()
                .Unauthorized();


        [Fact]
        public void GetDeleteShouldDeleteArticleAndRedirectWhenValidIdAndUserTriesToDelete()
           => MyController<ArticlesController>
               .Instance(instance => instance
                   .WithUser()
                   .WithData(ArticlesTestData.GetArticles(1)))
               .Calling(c => c.Delete(1))
               .ShouldHave()
               .Data(data => data
                   .WithSet<Article>(set => set.ShouldBeEmpty()))
               .AndAlso()
               .ShouldReturn()
               .Redirect(redirect => redirect
                   .To<ArticlesController>(c => c.Mine()));


        [Fact]
        public void GetDeleteShouldDeleteArticleAndRedirectWhenValidIdAndAdminTriesToDelete()
          => MyController<ArticlesController>
              .Instance(instance => instance
                   .WithUser(user => user.InRole("Administrator"))
                   .WithData(ArticlesTestData.GetArticles(1)))
              .Calling(c => c.Delete(1))
              .ShouldHave()
              .Data(data => data
                  .WithSet<Article>(set => set.ShouldBeEmpty()))
              .AndAlso()
              .ShouldReturn()
              .Redirect(redirect => redirect
                  .To<ArticlesController>(c => c.All(With.Empty<AllArticlesQueryModel>())));
    }

}
