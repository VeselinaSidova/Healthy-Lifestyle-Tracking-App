using HealthyLifestyleTrackingApp.Areas.Admin;
using MyTested.AspNetCore.Mvc;
using Xunit;

using LifeCoachesController = HealthyLifestyleTrackingApp.Areas.Admin.Controllers.LifeCoachesController;

namespace HealthyLifestyleTrackingApp.Test.Controllers.Admin
{
    public class LifeCoachesControllerTest
    {
        [Fact]
        public void ControllerShouldBeInAdminArea()
            => MyController<LifeCoachesController>
                .ShouldHave()
                .Attributes(attrs => attrs
                    .SpecifyingArea(AdminConstants.AreaName)
                    .RestrictingForAuthorizedRequests(AdminConstants.AdministratorRoleName));
    }
}
