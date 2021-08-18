using Xunit;
using MyTested.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Models.Tracker;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class TrackerControllerTest
    {
        [Fact]
        public void GetSelectDateShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tracker/SelectDate")
                .To<TrackerController>(c => c.SelectDate());

        [Fact]
        public void PostSelectDateShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Tracker/SelectDate")
                    .WithMethod(HttpMethod.Post))
                .To<TrackerController>(c => c.SelectDate(With.Any<DatePickerModel>()));

        [Fact]
        public void GetViewTrackedShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Tracker/ViewTracked/18-08-2021")
                .To<TrackerController>(c => c.ViewTracked(With.Any<AllTrackedQueryModel>(), "18-08-2021"));

        [Fact]
        public void GetDeleteTrackedFoodShouldBeMappedCorrectly()
             => MyRouting
                 .Configuration()
                 .ShouldMap("/Tracker/DeleteTrackedFood/1")
                 .To<TrackerController>(c => c.DeleteTrackedFood(1));

        [Fact]
        public void GetDeleteTrackedExerciseShouldBeMappedCorrectly()
             => MyRouting
                 .Configuration()
                 .ShouldMap("/Tracker/DeleteTrackedExercise/1")
                 .To<TrackerController>(c => c.DeleteTrackedExercise(1));

    }
}
