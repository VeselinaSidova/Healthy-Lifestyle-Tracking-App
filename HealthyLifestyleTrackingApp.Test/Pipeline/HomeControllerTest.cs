using Xunit;
using MyTested.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Controllers;

namespace HealthyLifestyleTrackingApp.Test.Pipeline
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnView()
           => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Index")
                .To<HomeController>(c => c.Index())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();
    }
}
