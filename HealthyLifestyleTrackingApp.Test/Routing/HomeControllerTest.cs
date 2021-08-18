using Xunit;
using MyTested.AspNetCore.Mvc;
using HealthyLifestyleTrackingApp.Controllers;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class HomeControllerTest
    {
        [Fact]
        public void GetIndexRouteShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index());

        [Fact]
        public void GetErrorRouteShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error());
    }
}
