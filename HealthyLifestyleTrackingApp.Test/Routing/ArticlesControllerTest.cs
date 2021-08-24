using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Models.Articles;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class ArticlesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Articles/All")
              .To<ArticlesController>(c => c.All(With.Any<AllArticlesQueryModel>()));

        [Fact]
        public void GetAllWithSearchTermShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Articles/All?SearchTerm=test")
               .To<ArticlesController>(c => c.All(With.Value(new AllArticlesQueryModel { SearchTerm = "test" })));

        [Fact]
        public void GetAllWithPageShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Articles/All?currentPage=1")
              .To<ArticlesController>(c => c.All(With.Value(new AllArticlesQueryModel { CurrentPage = 1 })));

        [Fact]
        public void GetAllWithPageAndSearchTermShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Articles/All?currentPage=1&searchTerm=test")
              .To<ArticlesController>(c => c.All(With.Value(new AllArticlesQueryModel { SearchTerm = "test", CurrentPage = 1 })));


        [Fact]
        public void GetMineShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Articles/Mine")
              .To<ArticlesController>(c => c.Mine());

        [Fact]
        public void GetReadShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Article/Read/1/Article")
              .To<ArticlesController>(c => c.Read(1, "Article"));


        [Fact]
        public void GetCreateShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Articles/Create")
               .To<ArticlesController>(c => c.Create());

        [Fact]
        public void PostCreateShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Articles/Create")
                    .WithMethod(HttpMethod.Post))
                .To<ArticlesController>(c => c.Create(With.Any<ArticleFormModel>()));

        [Fact]
        public void GetEditShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Article/Edit/1/Name")
              .To<ArticlesController>(c => c.Edit(1));

        [Fact]
        public void PostEditShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Article/Edit/1/Name")
                    .WithMethod(HttpMethod.Post))
                .To<ArticlesController>(c => c.Edit(1, With.Any<ArticleFormModel>()));

        [Fact]
        public void GetDeleteShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Articles/Delete/1")
             .To<ArticlesController>(c => c.Delete(1));
    }
}
