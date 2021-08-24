using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Data.Enums;
using HealthyLifestyleTrackingApp.Models.Exercises;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class ExercisesControllerTest
    {
        [Fact]
        public void GetAllShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Exercises/All")
              .To<ExercisesController>(c => c.All(With.Any<AllExercisesQueryModel>()));


        [Fact]
        public void GetAllWithSearchTermShouldBeRoutedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Exercises/All?Category=&Sorting=1&SearchTerm=test")
               .To<ExercisesController>(c => c.All(With.Value(new AllExercisesQueryModel { SearchTerm = "test" })));


        [Fact]
        public void GetAllWithPageShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Exercises/All?currentPage=1")
              .To<ExercisesController>(c => c.All(With.Value(new AllExercisesQueryModel { CurrentPage = 1 })));


        [Fact]
        public void GetAllWithPageAndSearchTermShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Exercises/All?currentPage=1&Category=&Sorting=1&SearchTerm=test")
              .To<ExercisesController>(c => c.All(With.Value(new AllExercisesQueryModel { SearchTerm = "test", CurrentPage = 1 })));


        [Fact]
        public void GetAllWithPageSearchTermAndCategoryShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Exercises/All?currentPage=1&Category=Light&Sorting=1&SearchTerm=test")
             .To<ExercisesController>(c => c.All(With.Value(new AllExercisesQueryModel {  Category = "Light", SearchTerm = "test", CurrentPage = 1 })));


        [Fact]
        public void GetAllWithPageSearchTermCategoryAndSortingShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Exercises/All?currentPage=1&Category=Light&Sorting=2&SearchTerm=test")
             .To<ExercisesController>(c => c.All(With.Value(new AllExercisesQueryModel { Category = "Light", SearchTerm = "test", CurrentPage = 1, Sorting = (Sorting)2 })));


        [Fact]
        public void GetCreateShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Exercises/Create")
               .To<ExercisesController>(c => c.Create());


        [Fact]
        public void PostCreateShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Exercises/Create")
                    .WithMethod(HttpMethod.Post))
                .To<ExercisesController>(c => c.Create(With.Any<ExerciseFormModel>()));


        [Fact]
        public void GetTrackShouldBeMappedCorrectly()
           => MyRouting
               .Configuration()
               .ShouldMap("/Exercise/Track/1/Name")
               .To<ExercisesController>(c => c.Track());


        [Fact]
        public void PostTrackShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Exercise/Track/1/Name")
                    .WithMethod(HttpMethod.Post))
                .To<ExercisesController>(c => c.Track(1, "Name", With.Any<TrackExerciseFormModel>()));


        [Fact]
        public void GetEditShouldBeMappedCorrectly()
          => MyRouting
              .Configuration()
              .ShouldMap("/Exercise/Edit/1/Name")
              .To<ExercisesController>(c => c.Edit(1));


        [Fact]
        public void PostEditShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Exercise/Edit/1/Name")
                    .WithMethod(HttpMethod.Post))
                .To<ExercisesController>(c => c.Edit(1, With.Any<ExerciseFormModel>()));


        [Fact]
        public void GetDeleteShouldBeMappedCorrectly()
         => MyRouting
             .Configuration()
             .ShouldMap("/Exercises/Delete/1")
             .To<ExercisesController>(c => c.Delete(1));
    }
}
