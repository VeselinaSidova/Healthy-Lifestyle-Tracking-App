using HealthyLifestyleTrackingApp.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace HealthyLifestyleTrackingApp.Test.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Index())
                .ShouldReturn()
                .View();

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
    }
}
