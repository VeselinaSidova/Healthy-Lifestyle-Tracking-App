using Xunit;
using MyTested.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Models.Recipes;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class RecipesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Recipes/All")
              .To<RecipesController>(c => c.All(With.Any<AllRecipesQueryModel>()));

        [Fact]
        public void GetAllWithSearchTermShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Recipes/All?SearchTerm=test")
               .To<RecipesController>(c => c.All(With.Value(new AllRecipesQueryModel { SearchTerm = "test" })));

        [Fact]
        public void GetAllWithPageShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Recipes/All?currentPage=1")
              .To<RecipesController>(c => c.All(With.Value(new AllRecipesQueryModel { CurrentPage = 1 })));

        [Fact]
        public void GetAllWithPageAndSearchTermShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Recipes/All?currentPage=1&searchTerm=test")
              .To<RecipesController>(c => c.All(With.Value(new AllRecipesQueryModel { SearchTerm = "test", CurrentPage = 1 })));


        [Fact]
        public void GetMineShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Recipes/Mine")
              .To<RecipesController>(c => c.Mine());

        [Fact]
        public void GetReadShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Recipe/Read/1/Recipe")
              .To<RecipesController>(c => c.Read(1, "Recipe"));


        [Fact]
        public void GetCreateShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Recipes/Create")
               .To<RecipesController>(c => c.Create());

        [Fact]
        public void PostCreateShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Recipes/Create")
                    .WithMethod(HttpMethod.Post))
                .To<RecipesController>(c => c.Create(With.Any<RecipeFormModel>()));

        [Fact]
        public void GetEditShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Recipe/Edit/1/Name")
              .To<RecipesController>(c => c.Edit(1));

        [Fact]
        public void PostEditShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Recipe/Edit/1/Name")
                    .WithMethod(HttpMethod.Post))
                .To<RecipesController>(c => c.Edit(1, With.Any<RecipeFormModel>()));

        [Fact]
        public void GetDeleteShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Recipes/Delete/1")
             .To<RecipesController>(c => c.Delete(1));
    }
}
