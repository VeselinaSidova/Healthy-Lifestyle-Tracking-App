using HealthyLifestyleTrackingApp.Controllers;
using HealthyLifestyleTrackingApp.Models.LifeCoaches;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace HealthyLifestyleTrackingApp.Test.Routing
{
    public class LifeCoachesControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap("/LifeCoaches/Become")
                .To<LifeCoachesController>(c => c.Become());

        [Fact]
        public void PostBecomeShouldBeMappedCorrectly()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/LifeCoaches/Become")
                    .WithMethod(HttpMethod.Post))
                .To<LifeCoachesController>(c => c.Become(With.Any<BecomeLifeCoachFormModel>()));
    }
}
