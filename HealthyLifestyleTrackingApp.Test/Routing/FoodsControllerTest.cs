using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Models.Foods;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class FoodsControllerTest
    {
        [Fact]
        public void GetAllShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Foods/All")
              .To<FoodsController>(c => c.All(With.Any<AllFoodsQueryModel>()));


        [Fact]
        public void GetAllWithSearchTermShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Foods/All?Category=&Tag=&Sorting=1&SearchTerm=test")
               .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { SearchTerm = "test" })));

        [Fact]
        public void GetAllWithCategoryShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Foods/All?Category=Ready+meals&Tag=&Sorting=1&SearchTerm=")
               .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { Category = "Ready meals" })));

        [Fact]
        public void GetAllWithTagShouldBeRoutedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Foods/All?Category=&Tag=Healthy&Sorting=1&SearchTerm=")
              .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { Tag = "Healthy" })));


        [Fact]
        public void GetAllWithPageShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Foods/All?currentPage=1")
              .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { CurrentPage = 1 })));


        [Fact]
        public void GetAllWithPageAndSearchTermShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Foods/All?currentPage=1&Category=&Tag=&Sorting=1&SearchTerm=test")
              .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { SearchTerm = "test", CurrentPage = 1 })));


        [Fact]
        public void GetAllWithPageSearchTermAndCategoryShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Foods/All?currentPage=1&Category=Light&Tag=&Sorting=1&SearchTerm=test")
             .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { Category = "Light", SearchTerm = "test", CurrentPage = 1 })));


        [Fact]
        public void GetAllWithPageSearchTermCategoryAndSortingShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Foods/All?currentPage=1&Category=Light&Tag=&Sorting=2&SearchTerm=test")
             .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { Category = "Light", SearchTerm = "test", CurrentPage = 1, Sorting = (Sorting)2 })));


        [Fact]
        public void GetAllWithPageSearchTermCategorySortingAndTagShouldBeMappedCorrectly()
        => MyRouting
            .Configuration()
            .ShouldMap("/Foods/All?currentPage=1&Category=Light&Tag=Alcohol&Sorting=2&SearchTerm=test")
            .To<FoodsController>(c => c.All(With.Value(new AllFoodsQueryModel { Category = "Light", SearchTerm = "test", CurrentPage = 1, Sorting = (Sorting)2, Tag = "Alcohol" })));


        [Fact]
        public void GetDetailsShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Food/Details/1/Food")
             .To<FoodsController>(c => c.Details(1, "Food"));


        [Fact]
        public void GetCreateShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Foods/Create")
               .To<FoodsController>(c => c.Create());


        [Fact]
        public void PostCreateShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Foods/Create")
                    .WithMethod(HttpMethod.Post))
                .To<FoodsController>(c => c.Create(With.Any<FoodFormModel>()));


        [Fact]
        public void GetTrackShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Food/Track/1/Name")
               .To<FoodsController>(c => c.Track());


        [Fact]
        public void PostTrackShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Food/Track/1/Name")
                    .WithMethod(HttpMethod.Post))
                .To<FoodsController>(c => c.Track(1, "Name", With.Any<TrackFoodFormModel>()));


        [Fact]
        public void GetEditShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Food/Edit/1/Name")
              .To<FoodsController>(c => c.Edit(1, "Name"));


        [Fact]
        public void PostEditShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Food/Edit/1/Name")
                    .WithMethod(HttpMethod.Post))
                .To<FoodsController>(c => c.Edit(1, With.Any<FoodFormModel>()));


        [Fact]
        public void GetDeleteShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Foods/Delete/1")
             .To<FoodsController>(c => c.Delete(1));
    }
}
